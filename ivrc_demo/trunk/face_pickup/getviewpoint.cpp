//author: kudo
//�֊s�擾�C��l�摜�o��
//���_�̍��W�C�ȉ~�t�B�b�e�B���O�摜���o��

//from 2011.09.23 �e�֊s�̏d�S���o��
//2011.09.24 �[�Ɍ����m�C�Y�֊s�������C�S�̂̏d�S��_���o��

#include "detection_thread.h"

#define BLOCK 55
#define MARK_THRESH -100

//using namespace cv;

cv::Point2d getviewpoint(cv::Mat gray_img){

	// �K���I��臒l����
	cv::Mat cont_img;
	// ���͉摜�C�o�͉摜�CmaxVal�C臒l�����@�C臒l������@�CblockSize�CConst
	cv::adaptiveThreshold(gray_img, cont_img, 255, cv::ADAPTIVE_THRESH_MEAN_C, cv::THRESH_BINARY, BLOCK, MARK_THRESH);
	//cv::threshold(gray_img, adaptive_img, 0, 255, cv::THRESH_BINARY|cv::THRESH_OTSU);

	//cv::imshow("bin_img_org", cont_img);

	vector<vector<cv::Point>> contours;
	// �֊s�̌��o(EXTERNAL or LIST)
	cv::findContours(cont_img, contours, CV_RETR_LIST, CV_CHAIN_APPROX_NONE);

	//��l�摜�̏o��
/*
	cv::drawContours(cont_img, contours, -1, cv::Scalar(100), 2, 8);
	cv::namedWindow("bin image", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	cv::imshow("bin image", cont_img);
	*/

	vector<cv::Point2d> averages;  
	cv::Point2d point;
  
	for(int i = 0; i < contours.size(); ++i) {
		size_t count = contours[i].size();
		if(count < 20 || count > 400) continue; //9/25�ύXyada
		//if(!isContourConvex(contours[i])) continue; //�ʂłȂ��֊s�����O
		//accumulate(contours[i].begin(), contours.end(), 0)
		cv::Moments mom = cv::moments(contours[i]);
		point.x = mom.m10/mom.m00;
		point.y = mom.m01/mom.m00;

		//�[�Ɍ����]�v�ȗ֊s���͂����i�㉺���E1/10�j//9/25�ύXyada
 		if( abs(point.x - gray_img.cols/2) > 2* gray_img.cols/5) continue;
		if( abs(point.y - gray_img.rows/2) > 2 * gray_img.rows/5) continue;

		averages.push_back(point);

		//�ȉ~�`��
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


	//�ȉ~�t�B�b�g�摜�̏o��
/*	cv::namedWindow("fit ellipse", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	cv::imshow("fit ellipse", gray_img);
	*/



	return viewpoint;
}
