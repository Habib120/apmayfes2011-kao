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
//
//double predict(IplImage *src)
//{
//
///**
//	/*****RandomTrees*****/
//	CvMat *test = cvCreateMat(1, size*40, CV_32FC1);
//	double r;
//	CvERTrees ert;
//	ert.load("./test2.xml");
//	//cout << ert.get_tree_count() << endl;
//	r = ert.predict(test);
//	cout << "認識結果：" << r << endl;
//
//	cvReleaseMat(&feat);
//	return r;
//}





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