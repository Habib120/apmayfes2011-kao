//2011.09.23
#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <math.h>
#include <iostream>

#define BLOCK 55
#define MARK_THRESH -80

//using namespace cv;
using std::cout;
using std::endl;
using std::vector;

vector<cv::Point> imageio(cv::Mat gray_img){
  //cv::Mat src_img = cv::imread("../../image/lenna.png", 1);
  //if(!src_img.data) return -1;
	/*cv::Mat gray_img = cv::imread("../image/IRtest.jpg", 0);*/
	//if(!gray_img.data) return -1;

	// 適応的な閾値処理
	cv::Mat adaptive_img;
	// 入力画像，出力画像，maxVal，閾値決定手法，閾値処理手法，blockSize，Const
	cv::adaptiveThreshold(gray_img, adaptive_img, 255, cv::ADAPTIVE_THRESH_MEAN_C, cv::THRESH_BINARY, BLOCK, MARK_THRESH);
	//cv::threshold(gray_img, adaptive_img, 0, 255, cv::THRESH_BINARY|cv::THRESH_OTSU);

	cv::Mat bin_img = adaptive_img;

	vector<vector<cv::Point> > contours;
	// 輪郭の検出(EXTERNAL or LIST)
	cv::findContours(adaptive_img, contours, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_NONE);

	vector<cv::Point> averages;  
	cv::Point point;
  
	for(int i = 0; i < contours.size(); ++i) {
		size_t count = contours[i].size();
		if(count < 40 || count > 500) continue; // （小さすぎる|大きすぎる）輪郭を除外
		//if(!isContourConvex(contours[i])) continue;
		//accumulate(contours[i].begin(), contours.end(), 0)
		cv::Moments mom = cv::moments(contours[i]);
		point.x = mom.m10/mom.m00;
		point.y = mom.m01/mom.m00;
		averages.push_back(point);
	    cv::Mat pointsf;
		cv::Mat(contours[i]).convertTo(pointsf, CV_32F);
		//// 楕円フィッティング
		//cv::RotatedRect box = cv::fitEllipse(pointsf);
		//// 楕円の描画
		//cv::ellipse(gray_img, box, cv::Scalar(0,0,255), 2, CV_AA);
	}

	//for( int i = 0; i < averages.size(); ++i )
	//{
	//	cout << averages[i] << endl;
	//}


	//// CV_WINDOW_AUTOSIZE : ウィンドウサイズを画像サイズに合わせる
	//cv::namedWindow("fit ellipse", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	//cv::namedWindow("bin image", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	//
	//// ウィンドウ名でウィンドウを指定して，そこに画像を描画
	//cv::imshow("fit ellipse", gray_img);
	//cv::imshow("bin image", bin_img);
	
	// キー入力を（無限に）待つ
	/*cv::waitKey(0);*/

	return averages;
}
