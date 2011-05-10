#include "util.h"
#include "structure.h"

/**
 * 画像を回転させるロジック
 */
void ImageUtils::rotateImage( IplImage *img, double angle, cv::Point2f center)
{
	IplImage *tmp = cvCreateImage(cvGetSize(img), IPL_DEPTH_8U, 3);
	CvMat M;
	float m[6];

	m[0] = (float) (cos (angle));
	m[1] = (float) (-sin (angle));
	m[2] = center.x;
	m[3] = -m[1];
	m[4] = m[0];
	m[5] = center.y;
	cvInitMatHeader (&M, 2, 3, CV_32FC1, m, CV_AUTOSTEP);
	cvGetQuadrangleSubPix(img,tmp,&M);

	cvCopy(tmp, img, NULL);
	cvReleaseImage(&tmp);
}

/**
 * smAPIの顔姿勢データを基に、カメラ画像から顔領域を抽出する
 */
IplImage* ImageUtils::clipHeadImage(IplImage* src, HeadPose pose)
{
	//顔が検出されていない場合は何も返さない
	if (!pose.isValueSet())
	{
		return 0;
	}
	int center_row,center_col,start_col,end_col,start_row,end_row;
	cv::Point2f center;

	cv::Size src_size(cvGetSize(src));
	center_row = src_size.height/2 + (25 - pose.y*(src_size.height/0.4)*cos(pose.rot))*0.4/pose.z;
	center_col = src_size.width/2 + (((pose.x*src_size.width+4)/0.4)*cos(pose.rot) + pose.y*(src_size.height/0.3)*sin(pose.rot))*0.4/pose.z;
	center.x=center_col;
	center.y=center_row;
	//cout << src_size.height << endl;

	cv::Size sz;
	sz.width = 2*floor(src_size.height/(pose.z*10));
	//cout << src_size.width << endl;
	sz.height = sz.width;
	start_col = center_col - sz.width/2;
	end_col = center_col + sz.width/2;
	start_row = center_row - sz.height/2;
	end_row = center_row + sz.height/2;
	//rot = cv::getRotationMatrix2D(center,rad,1);
	//mat.create(src_size.height,src_size.width,CV_16SC3);
	cvFlip(src, src, CV_16SC3);
	ImageUtils::rotateImage(src,-pose.rot,center);
	cv::Mat dst(src);
	cv::Mat dst2 = dst;	
	
	if(start_row > 0 && end_row < src_size.height-20 && start_col > 10 && end_col < src_size.width-30){
		dst = dst.rowRange(src_size.height/2-sz.height/2,src_size.height/2+sz.height/2);
		dst = dst.colRange(src_size.width/2-sz.width/2,src_size.width/2+sz.width/2);
			//cv::getRectSubPix(mat,src_size,center,dst);			
		//cv::imshow("Capture", dst);

		double r=0;
		cv::cvtColor(dst,dst2,CV_RGB2GRAY,0);
		IplImage dimg = dst2;
		IplImage *result;
		result = cvCreateImage(cvSize(128,128), IPL_DEPTH_8U, 1);
		cvResize(&dimg, result, CV_INTER_LINEAR);
		return result;
		//cout << "result" << r << endl;
	}
	else
	{
		return 0;
	}
}
