//---------------------------------------------------------------
//author: kudo
//画像を２値化し，輪郭を取得
//輪郭の中心をマーカー点をする
//マーカー点の重心として視点座標を出力

//2011.09.23 村下のC#コードを移植．各輪郭の重心を出力
//09.24 端に現れるノイズ輪郭を除去，全体の重心一点を出力
//09.28 デバッグ関連を追加，メソッドをxyとzに分離
//09.29 余計な点を除去
//10.01 除去する点をxy: 右端, z: 左端に設定
//---------------------------------------------------------------

#include "detection_thread.h"

//#define DEBUG

#define BLOCK 55
#define MARK_THRESH -100

//using namespace cv;
using namespace std;

vector<cv::Point2d> getviewpoint(cv::Mat gray_img){

	// 適応的な閾値処理
	cv::Mat cont_img;
	// 引数（入力画像，出力画像，maxVal，閾値決定手法，閾値処理手法，blockSize，Const）
	cv::adaptiveThreshold(gray_img, cont_img, 255, cv::ADAPTIVE_THRESH_MEAN_C, cv::THRESH_BINARY, BLOCK, MARK_THRESH);
	//cv::threshold(gray_img, adaptive_img, 0, 255, cv::THRESH_BINARY|cv::THRESH_OTSU);
	//cv::threshold(gray_img, cont_img, 200, 255, cv::THRESH_BINARY);
	vector<vector<cv::Point>> contours;
	// 輪郭の検出(EXTERNAL or LIST)
	cv::findContours(cont_img, contours, CV_RETR_LIST, CV_CHAIN_APPROX_NONE);

#ifdef DEBUG
	//二値画像の出力
	cv::drawContours(cont_img, contours, -1, cv::Scalar(100), 2, 8);
	cv::namedWindow("bin image", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	cv::imshow("bin image", cont_img);
#endif 

	//マーカー点をaveragesに格納する
	vector<cv::Point2d> averages;  
	cv::Point2d point;

	for(int i = 0; i < contours.size(); ++i) {
		size_t count = contours[i].size();
		
		//小さすぎる・大きすぎる輪郭を除く
#ifdef DEBUG
		if(count < 50 || count > 300) continue;
#else
		if(count < 30 || count > 100) continue;  //9/25変更yada
#endif

		cv::Moments mom = cv::moments(contours[i]);
		point.x = mom.m10/mom.m00;
		point.y = mom.m01/mom.m00;

		//端に現れる余計な輪郭をはじく（10/01変更）
		if( abs(point.x - gray_img.cols/2) > 4* gray_img.cols/9) continue; //左右1/18
		if( abs(point.y - gray_img.rows/2) > 2 * gray_img.rows/5) continue; //上下1/10
		if( (point.y - gray_img.rows/2) > 3 * gray_img.rows/10) continue; //下側1/5

		//マーカー点に追加
		averages.push_back(point);

		//楕円描画
#ifdef DEBUG
		cv::Mat pointsf;
		cv::Mat(contours[i]).convertTo(pointsf, CV_32F);
		cv::RotatedRect box = cv::fitEllipse(pointsf);
		cv::ellipse(gray_img, box, cv::Scalar(0,0,255), 2, CV_AA);
#endif
	}
	//楕円フィット画像の出力
#ifdef DEBUG
	cv::namedWindow("fit ellipse", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	cv::imshow("fit ellipse", gray_img);
#endif
	return averages;
}


//昇順ソートの条件を返す関数（a.x >b.xなら降順）
bool ascendingorder(cv::Point2d a, cv::Point2d b){
	if (a.x == b.x)
		return a.y < b.y;
	else
		return a.x < b.x;
}

//降順ソート
bool descendingorder(cv::Point2d a, cv::Point2d b){
	if (a.x == b.x)
		return a.y < b.y;
	else
		return a.x > b.x;
}


cv::Point2d getpointxy(vector<cv::Point2d> averages){
	//x座標昇順ソート
	std::sort (averages.begin(), averages.end(), ascendingorder);
	
	//端の２点が近すぎる場合，ヘッドホン後側のマーカーとみなして除去(基準比率1/5)
	int n = averages.size();
	if( (averages[n-1].x - averages[0].x) > 5*(averages[n-1].x - averages[n-2].x) ){
		averages.pop_back();
	};
	
	//重心を求める
	cv::Point2d sum;// = accumulate( averages.begin(), averages.end(), 0 ); /// averages.size();
	for( int i = 0; i < averages.size(); ++i ){
		sum += averages[i];
#ifdef DEBUG
		cout << averages[i] << endl;
#endif
	}
	
	cv::Point2d viewpoint = sum*(1.0/averages.size());
	return viewpoint;
}

cv::Point2d getpointz(vector<cv::Point2d> averages){
	//反対側のマーカーが検出された場合の動作
	if(averages.size() > 2){
		//x座標降順ソート
		std::sort (averages.begin(), averages.end(), descendingorder);
		//一番右端（x座標小）の点を除く
		averages.pop_back();
	}

	//重心を求める
	cv::Point2d sum;
	for( int i = 0; i < averages.size(); ++i ){
		sum += averages[i];
#ifdef DEBUG
		cout << averages[i] << endl;
#endif
	}

	cv::Point2d viewpoint = sum*(1.0/averages.size());
	return viewpoint;
}


cv::Point2d getviewpointxy(cv::Mat gray_img){
	vector<cv::Point2d> averages = getviewpoint(gray_img);
	//9/27追加 yada
	if(averages.size() < 2 ){
		return cv::Point2d(-1,-1);
	}
	cv::Point2d viewpoint = getpointxy(averages);
	return(viewpoint);
}

cv::Point2d getviewpointz(cv::Mat gray_img){
	vector<cv::Point2d> averages = getviewpoint(gray_img);

	//9/27追加 yada
	if(averages.size() < 2 ){
		return cv::Point2d(-1,-1);
	}
	cv::Point2d viewpoint = getpointz(averages);
	return(viewpoint);
}
