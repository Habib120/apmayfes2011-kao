//author: kudo
//輪郭取得，二値画像出力
//視点の座標，楕円フィッティング画像を出力

//from 2011.09.23 各輪郭の重心を出力
//2011.09.24 端に現れるノイズ輪郭を除去，全体の重心一点を出力

#include "detection_thread.h"

#define BLOCK 55
#define MARK_THRESH -100

//using namespace cv;

cv::Point2d getviewpoint(cv::Mat gray_img){

	// 適応的な閾値処理
	cv::Mat cont_img;
	// 入力画像，出力画像，maxVal，閾値決定手法，閾値処理手法，blockSize，Const
	cv::adaptiveThreshold(gray_img, cont_img, 255, cv::ADAPTIVE_THRESH_MEAN_C, cv::THRESH_BINARY, BLOCK, MARK_THRESH);
	//cv::threshold(gray_img, adaptive_img, 0, 255, cv::THRESH_BINARY|cv::THRESH_OTSU);

	//cv::imshow("bin_img_org", cont_img);

	vector<vector<cv::Point>> contours;
	// 輪郭の検出(EXTERNAL or LIST)
	cv::findContours(cont_img, contours, CV_RETR_LIST, CV_CHAIN_APPROX_NONE);

	//二値画像の出力
/*
	cv::drawContours(cont_img, contours, -1, cv::Scalar(100), 2, 8);
	cv::namedWindow("bin image", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	cv::imshow("bin image", cont_img);
	*/

	vector<cv::Point2d> averages;  
	cv::Point2d point;
  
	for(int i = 0; i < contours.size(); ++i) {
		size_t count = contours[i].size();
		if(count < 20 || count > 400) continue; //9/25変更yada
		//if(!isContourConvex(contours[i])) continue; //凸でない輪郭を除外
		//accumulate(contours[i].begin(), contours.end(), 0)
		cv::Moments mom = cv::moments(contours[i]);
		point.x = mom.m10/mom.m00;
		point.y = mom.m01/mom.m00;

		//端に現れる余計な輪郭をはじく（上下左右1/10）//9/25変更yada
 		if( abs(point.x - gray_img.cols/2) > 2* gray_img.cols/5) continue;
		if( abs(point.y - gray_img.rows/2) > 2 * gray_img.rows/5) continue;

		averages.push_back(point);

		//楕円描画
/*	    cv::Mat pointsf;
		cv::Mat(contours[i]).convertTo(pointsf, CV_32F);
		cv::RotatedRect box = cv::fitEllipse(pointsf);
		cv::ellipse(gray_img, box, cv::Scalar(0,0,255), 2, CV_AA);
		*/
	}

	cv::Point2d sum;// = accumulate( averages.begin(), averages.end(), 0 ); /// averages.size();

	for( int i = 0; i < averages.size(); ++i )
	{
		sum.x += averages[i].x;
		sum.y += averages[i].y;
//		cout << averages[i] << endl;
	}
	cv::Point2d viewpoint;
	viewpoint.x = sum.x/averages.size();
	viewpoint.y = sum.y/averages.size();


	//楕円フィット画像の出力
/*	cv::namedWindow("fit ellipse", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	cv::imshow("fit ellipse", gray_img);
	*/



	return viewpoint;
}
