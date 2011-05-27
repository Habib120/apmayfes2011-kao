#include <boost/asio.hpp>
#include "detectors.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <boost/regex.hpp>
#include <boost/lexical_cast.hpp>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/json_parser.hpp>
#include <boost/foreach.hpp>
#include <ctime>
#include <cstdlib>
#include <sstream>

using namespace std;
using namespace boost::asio;
using namespace boost::property_tree;

std::string FaceComDetector::ipaddress;
std::string FaceComDetector::host;
std::string FaceComDetector::boundary;
std::string FaceComDetector::header_str;
std::string FaceComDetector::footer_str;
size_t FaceComDetector::header_size = 0;
size_t FaceComDetector::footer_size = 0;

void FaceComDetector::initialize()
{
	host = "api.face.com";
	/**
	 * ホスト名からIPアドレスを取得する
	 */
	io_service io_service;
	ip::tcp::resolver resolver(io_service);
	ip::tcp::resolver::query query(host, "http");
	ip::tcp::resolver::iterator endpoint_iterator = resolver.resolve(query);
	ip::tcp::resolver::iterator end;
    for (; endpoint_iterator != end; ++endpoint_iterator) {
        ipaddress = endpoint_iterator->endpoint().address().to_string();
    }
	/**
	 * bodyの内容はファイル以外共通なので、static領域に作成しておく
	 */
	boundary = "---------------------------7d97d32d00e4";

	std::stringstream h_stream;
	h_stream << "--" << boundary << "\r\n";
	h_stream << "Content-Disposition: form-data; name=\"api_key\"\r\n";
	h_stream << "\r\n"; //Content-Dispositionと値の宣言の間には必ず改行を挟む
	h_stream << "cfd89d59c0681a7d9e26573c223a46c9\r\n";
	h_stream << "--" << boundary << "\r\n";
	h_stream << "Content-Disposition: form-data; name=\"api_secret\"\r\n";
	h_stream << "\r\n";
	h_stream << "1bb0b5a9dee6a4030dd159909df7b274\r\n";
	h_stream << "--" << boundary << "\r\n";
	h_stream << "Content-Disposition: form-data; name=\"_file\"; filename=\"test.jpg\"\r\n";
	h_stream << "Content-Type: image/jpg\r\n";
	h_stream << "\r\n";

	std::stringstream f_stream;
	f_stream << "\r\n--" << boundary << "--\r\n"; //本文の終わりの宣言--boundary--の前後に改行を忘れないこと

	header_size = (size_t)h_stream.seekg(0, std::ios::end).tellg();
	footer_size = (size_t)f_stream.seekg(0, std::ios::end).tellg();

	header_str = h_stream.str();
	footer_str = f_stream.str();
}

std::vector<FaceComDetectionResult> FaceComDetector::Detect(HeadData data)
{
	return this->Detect(data.GetImage());
}

std::vector<FaceComDetectionResult> FaceComDetector::Detect(IplImage* image)
{
	std::vector<FaceComDetectionResult> results;
	if (header_size == 0)
		initialize();
	
	//顔画像データの保存
	time_t t = time(0);
	tm *x = localtime(&t);
	std::stringstream stream;
	stream << "face_" << x->tm_mday << "_" << x->tm_hour << "_" << x->tm_min << "_" << x->tm_sec << "_" << rand() << ".jpg";
	std::string filename = std::string("c:\\tmp\\") + stream.str();
	cvSaveImage(filename.c_str(), image);

	//ファイルストリームを開く
	ifstream f(filename.c_str() ,ios::in | ios_base::binary);
	if (!f){
		cerr << "ファイルが開けません "<< endl;
		return results;
	}

	//画像サイズの取得
	size_t filesize = (size_t)f.seekg(0, std::ios::end).tellg();
	f.seekg(0, std::ios::beg);  

	//TCP接続
	ip::tcp::iostream s( ipaddress, "80" );
	if(s.fail()){
        cerr << "Webサーバが応答しません" << endl;
		return results;
	}

	//POSTデータの書き込み
	s << "POST http://api.face.com/faces/detect.json HTTP/1.1\r\n";
	s << "Content-Type: multipart/form-data; boundary=" << boundary << "\r\n";
	s << "Host: ";
	s << ipaddress;
	s << ":80\r\n";
	s << "Content-Length:" << header_size + footer_size + filesize << "\r\n";
	s << "Connection: Close\r\n";
	s << "Cache-Control: no-cache\r\n";
	s << "\r\n";

	s << header_str;
    char buffer[1024];
	int readsize = 0;
	while(!f.eof()){
		f.read(buffer,sizeof(buffer));
		s.write(buffer,f.gcount());
	}
	s << footer_str;
	s << flush;

	//レスポンスのチェック
	boost::regex reg("([0-9]{3})");
	string line;
	int statuscode;
	getline(s, line);
	boost::smatch result;
	if(boost::regex_search(line, result, reg)){//ステータスコードがある場合
		statuscode = boost::lexical_cast<int>(result.str(1));//数値へ変換
	}
	if(statuscode != 200){
		cerr << "Error connecting face api -- statuscode : "  << statuscode << endl;
		return results;
	}

	//jsonデータの取得
	//ヘッダーを読み飛ばす
	while (getline(s, line))
	{
		if (line == "\r")
			break;
	}
	std::stringstream jsondata;
	while (getline(s, line))
	{
		jsondata << line << endl;
	}
	//cout << jsondata.str() << endl;

	//本文の取得
	ptree pt;
	read_json(jsondata, pt);
	if (pt.get<string>("status") == "failure")
	{
		return results;
	}

	BOOST_FOREACH(ptree::value_type &v, pt.get_child("photos"))
	{
		BOOST_FOREACH(ptree::value_type &v2, v.second.get_child("tags"))
		{
			FaceComDetectionResult result;
			result.is_male = v2.second.get<string>("attributes.gender.value") == "male";
			result.con_gender = v2.second.get<double>("attributes.gender.confidence");
			result.is_smiling = v2.second.get<bool>("attributes.smiling.value");
			result.con_smiling = v2.second.get<double>("attributes.smiling.confidence");
			result.has_glasses = v2.second.get<bool>("attributes.glasses.value");
			result.con_glasses = v2.second.get<double>("attributes.glasses.confidence");
			double width = v2.second.get<double>("width");
			double height = v2.second.get<double>("height");
			double cx = v2.second.get<double>("center.x");
			double cy = v2.second.get<double>("center.y");
			CvRect rect = cvRect((int)(cx - width/2), (int)(cy - height/2), (int)width, (int)height);
			result.face_rect = rect;
			
			results.push_back(result);
		}
	}

	return results;
}
