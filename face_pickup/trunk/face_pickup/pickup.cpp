//#define _CRTDBG_MAP_ALLOC

#include <stdio.h>
#include <cv.h>
#include <highgui.h>
#include <ctype.h>
#include <iostream>
//#include <sm_api_cxx.h>
#include <conio.h>
#include "sm_api.h"
#include <cmath>
#include <time.h>
#include <ml.h>
#include "cvgabor.h"
#include "structure.h"
#include "util.h"
//#include <stdlib.h>
//#include <crtdbg.h>

using std::cout;
using std::endl;
using std::cerr;
using std::cin;
using std::string;
//using namespace sm::faceapi;

HeadPose pose;

// グローバルな演算子オーバーロード
std::ostream& operator<<(std::ostream& os, const HeadPose pose)
{
	return ( os << "HeadPose : (x, y, z, rot) = (" << pose.x << "," << pose.y<< "," << pose.z << "," << pose.rot << ")" );
}

double predict(IplImage *src)
{
	//cvNamedWindow("test1",1);
	//cvShowImage("test1",&dimg);
    //clock_t start, end;
    //cout << "初期化: ";
    //start = clock();
    //IplImage *src=0;
	//IplImage *input=0;
	//input = &dimg;
    //input = cvLoadImage( "./face_0043.jpg" , CV_LOAD_IMAGE_GRAYSCALE);
    //src = cvCreateImage(cvSize(128,128), IPL_DEPTH_8U, 1);
	//dst = cv/CreateImage(cvSize(128,128), IPL_DEPTH_8U, 1);
    int size = 400;

	//cvResize(input, src, CV_INTER_LINEAR); //画像を縮小
	
	CvMat *mat  = cvCreateMat(src->width, src->height, CV_32FC1); //画像と同じサイズの行列を生成
	CvMat *smat = cvCreateMat(20, 20, CV_32FC1); //縮小先の行列を生成
	CvMat *feat = cvCreateMat(1, size*40, CV_32FC1); //特徴量を格納するベクトルを生成
	CvMat row_header, *row;
	CvMat tmp;
	
	//cvNamedWindow("test2");
	//cvShowImage("test1",input);

	int k = 0;
	for (int u = 0; u < 5; u++)
	{
		for (int v = 0; v < 8; v++)
		{
			//cvShowImage("test1",&src);
			CvGabor gabor(u, v);
			//gabor.conv_img(src, dst);
			//cvNamedWindow("dst");
			//cvShowImage("dst", dst);
			//cvWaitKey();
			//cvDestroyWindow("dst");
			gabor.get_value(src, mat); //Gabor特徴量をmatに格納
			//cvShowImage("test2", mat);
			//for (int n = 0; n < 10; n++)
			//{
			//	cout << CV_MAT_ELEM(*mat, float, 0, n) << endl;
			//}
			cvResize(mat, smat, CV_INTER_LINEAR); //行列を縮小
			row = cvReshape(smat, &row_header, 0, 1); //行列を行ベクトルに変形
			//cout << CV_MAT_ELEM(*row, float, 0, size-1) << endl;
			cvGetCols(feat, &tmp, size*k, size*(k+1));
			cvCopy(row, &tmp); //featの一部に特徴量を並べた行ベクトルをコピー
			k = k+1;
		}
	}
	//cout << CV_MAT_ELEM(*feat, float, 0, size-1) << endl;

	//cvNamedWindow("src");
	//cvShowImage("src", src);
	//cvWaitKey();
	//cvDestroyWindow("src");
	//cvReleaseImage(&input);
	cvReleaseImage(&src);
	//cvReleaseImage(&dst);
	cvReleaseMat(&mat);
	cvReleaseMat(&smat);

			/*****RandomTrees*****/
			CvMat *test = cvCreateMat(1, size*40, CV_32FC1);
			double r;
			CvERTrees ert;
			ert.load("./test2.xml");
			//cout << ert.get_tree_count() << endl;
			r = ert.predict(test);
			cout << "認識結果：" << r << endl;

	cvReleaseMat(&feat);

	return r;
}




void STDCALL recieveHeadPose(void *,smEngineHeadPoseData head_pose, smCameraVideoFrame video_frame)
{
    smImageInfo video_frame_image_info;
    smImageGetInfo(video_frame.image_handle, &video_frame_image_info);     
	pose.x = head_pose.head_pos.x;
	pose.y = head_pose.head_pos.y;
	pose.z = head_pose.head_pos.z;
	pose.rot = head_pose.head_rot.z_rads;
}

int main ()
{
  //_CrtDumpMemoryLeaks();
  cv::Point2f center;
  cv::Mat mat,dst;
  cv::Size sz,cam_sz;

  cv::VideoCapture *cap = NULL;
  cap = new cv::VideoCapture(CV_CAP_ANY);
  if(!cap->isOpened()) 
        return -1;

  smAPIInit();	//faceAPI初期化	
  smCameraRegisterType(SM_API_CAMERA_TYPE_WDM);	//カメラタイプの登録（必須）
	
  //faceAPIエンジン作成	
  smEngineHandle engine = 0;	
  smEngineCreate(SM_API_ENGINE_TYPE_HEAD_TRACKER_V2,&engine);	
  smHTRegisterHeadPoseCallback(engine,0,recieveHeadPose);
  //faceAPI表示ディスプレイ作成
  smVideoDisplayHandle video_display_handle = 0;
  smVideoDisplayCreate(engine, &video_display_handle,0,TRUE);
  smVideoDisplaySetFlags(video_display_handle,SM_API_VIDEO_DISPLAY_HEAD_MESH);

  //faceAPIトラッキング開始
  smEngineStart(engine);

  cv::namedWindow("Capture",CV_WINDOW_AUTOSIZE);
  (*cap) >> mat;


  for(;;)
    {
		smAPIProcessEvents();
        const int frame_period_ms = 30;
        Sleep(frame_period_ms);
		if( pose.isValueSet() )
		{   
			(*cap) >> mat;
			IplImage cam_img = mat;
			IplImage* face_img = ImageUtils::clipHeadImage(&cam_img, pose);
			cvShowImage("Capture", face_img);
		}

		if(cv::waitKey(30) >= 0){
			break;
		}
  }
  smEngineDestroy(&engine);
  smVideoDisplayDestroy(&video_display_handle);

  smAPIQuit();

  cvDestroyWindow("Capture");
  
  return 0;
}