//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// This file is part of CL-EyeMulticam SDK
//
// C++ CLEyeMulticamTest Sample Application
//
// For updates and file downloads go to: http://codelaboratories.com/research/view/cl-eye-muticamera-sdk
//
// Copyright 2008-2010 (c) Code Laboratories, Inc. All rights reserved.
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#include "detection_thread.h"

#define _USE_MATH_DEFINES


#include "getviewpoint.h"
#include <vector>
#include <iostream>
#include <queue>
#include <deque>
#include <algorithm>


#define THETA M_PI/6
#define BUFSIZE 5

using std::cout;
using std::endl;
using std::vector;
using std::deque;
using std::max_element;

HANDLE mutex;
//HANDLE _hThread[2];
double cam_coord[3] = {0, 0, 0};
double coord[3] = {0, 0, 0};
//queue<double> coord_x;
//queue<double> coord_y;
//queue<double> coord_z;
double output[3];
int cam_select;



CLEyeCameraCapture::CLEyeCameraCapture(LPSTR windowName, GUID cameraGUID, CLEyeCameraColorMode mode, CLEyeCameraResolution resolution, float fps) :
	_cameraGUID(cameraGUID), _cam(NULL), _mode(mode), _resolution(resolution), _fps(fps), _running(false)
{
	strcpy(_windowName, windowName);
}
	
bool CLEyeCameraCapture::StartCapture()
{
	_running = true;
	cvNamedWindow(_windowName, CV_WINDOW_AUTOSIZE);
	// Start CLEye image capture thread
	_hThread = CreateThread(NULL, 0, &CLEyeCameraCapture::CaptureThread, this, 0, 0);
	if(_hThread == NULL)
	{
		//MessageBox(NULL,"Could not create capture thread","CLEyeMulticamTest", MB_ICONEXCLAMATION);
		return false;
	}
	Point_out.x = 0;
	Point_out.y = 0;
	return true;
}

void CLEyeCameraCapture::StopCapture()
{
	if(!_running)	return;
	_running = false;
	WaitForSingleObject(_hThread, 1000);
	cvDestroyWindow(_windowName);
}

void CLEyeCameraCapture::Run()
{
	int w, h;
		IplImage *pCapImage;
		PBYTE pCapBuffer = NULL;
		// Create camera instance
		_cam = CLEyeCreateCamera(_cameraGUID, _mode, _resolution, _fps);
		if(_cam == NULL)		return;
		// Get camera frame dimensions
		CLEyeCameraGetFrameDimensions(_cam, w, h);
		// Depending on color mode chosen, create the appropriate OpenCV image
		if(_mode == CLEYE_COLOR_PROCESSED || _mode == CLEYE_COLOR_RAW)
			pCapImage = cvCreateImage(cvSize(w, h), IPL_DEPTH_8U, 4);
		else
			pCapImage = cvCreateImage(cvSize(w, h), IPL_DEPTH_8U, 1);

		// Set some camera parameters
		CLEyeSetCameraParameter(_cam, CLEYE_AUTO_GAIN, true);
		CLEyeSetCameraParameter(_cam, CLEYE_AUTO_EXPOSURE, true);
		CLEyeSetCameraParameter(_cam, CLEYE_AUTO_WHITEBALANCE, true);
		CLEyeSetCameraParameter(_cam, CLEYE_ZOOM, 0);
 		CLEyeSetCameraParameter(_cam, CLEYE_ROTATION, 0);
 		//CLEyeSetCameraParameter(_cam, CLEYE_ZOOM, (int)(GetRandomNormalized()*100.0));
 		//CLEyeSetCameraParameter(_cam, CLEYE_ROTATION, (int)(GetRandomNormalized()*300.0));

		// Start capturing
		CLEyeCameraStart(_cam);
		cvGetImageRawData(pCapImage, &pCapBuffer);
		// image capturing loop
		while(_running)
		{
			CLEyeCameraGetFrame(_cam, pCapBuffer);
			cvFlip(pCapImage, pCapImage, 1);
			//cvShowImage(_windowName, pCapImage);
			CaptureMat = pCapImage;
			if ( cam_num == 0 ){
				Point = getviewpointxz(CaptureMat);
			}
			else if( cam_num == 1 ){
				Point = getviewpointy(CaptureMat);
			}
			
			//10/1デバッグ用//
			/*
			if ( Point.x >0 && Point.x <500) {
				Point_out = Point;
			}*/
			
			//9/30変更//
			//出力制御部分
			if ( Point.x > 0 && Point.x < 640/*Point != cv::Point2d(-1,-1)*/) {
				Point_buf.push_back(Point);
				if( Point_buf.size() < BUFSIZE){ //開始からBUFSIZEフレームに達するまでは入力をそのまま出力
					Point_out = Point;
				}else{
					Point_buf.pop_front();
					//入力が１フレーム前の出力と20ピクセル四方以内なら入力をそのまま出力
					if ( abs(Point.x-Point_out.x) < 20 && abs(Point.y-Point_out.y) < 20){	
						Point_out = Point;
					}
					//そうでないなら前BUFSIZE分のフレームの平均を出力
					else{
						Point_out.x = 0; Point_out.y = 0;
						for(deque<cv::Point2d>::iterator it = Point_buf.begin(); it != Point_buf.end(); ++it){
							Point_out.x += it->x;
							Point_out.y += it->y;
						}
						Point_out.x = Point_out.x/Point_buf.size();
						Point_out.y = Point_out.y/Point_buf.size();
					}
					/*else{
						Point_out.x = 0; Point_out.y = 0;
						Point_ave.x = 0; Point_ave.y = 0;
						for(deque<cv::Point2d>::iterator it = Point_buf.begin(); it != Point_buf.end(); ++it){
							Point_ave.x += it->x;
							Point_ave.y += it->y;
							//Point_buf2.push_back(*it);
						}
						Point_ave.x = Point_ave.x/Point_buf.size();
						Point_ave.y = Point_ave.y/Point_buf.size();
						//Point_buf2 = Point_buf;
						for(deque<cv::Point2d>::iterator it2 = Point_buf2.begin(); it2 != Point_buf2.end(); ++it2){
							var_tmp = pow((Point_ave.x-it2->x), 2.0) + pow((Point_ave.y-it2->y), 2.0);
							if(var_tmp > 500)
							{
								*it2 = Point_ave;
								//Point_buf2.insert(it2, Point_ave);
								//Point_buf2.erase(it2);	
							}
						}
						for(deque<cv::Point2d>::iterator it2 = Point_buf2.begin(); it2 != Point_buf2.end(); ++it2){
							Point_out.x += it2->x;
							Point_out.y += it2->y;
						}
						Point_out.x = Point_out.x/Point_buf2.size();
						Point_out.y = Point_out.y/Point_buf2.size();
					}*/
				}
			}else{
				if( !Point_buf.empty() ){
				while( !Point_buf.empty() )
					Point_buf.pop_front();
				}
			}
			WaitForSingleObject(mutex, INFINITE);
			if(cam_num == 0 ){//10.05 19:30 カメラ配置の変更(kudo)

				cam_coord[0] = Point_out.x-320.0;//x
				cam_coord[2] = Point_out.y-240.0;//z（斜めなのでやや不正確）
			}else if(cam_num == 1){
				cam_coord[1] = Point_out.y-240.0;//y
			}
			cvCircle(pCapImage, Point_out, 10, CV_RGB(0,0,0),-1);
			cvShowImage(_windowName, pCapImage);
			//    //

			//cout << "(x ,y ,z) = (" << coord[0] << ", " << coord[1] << ", " << coord[2] <<  ")" << endl;
			ReleaseMutex(mutex);
		}
		// Stop camera capture
		CLEyeCameraStop(_cam);
		// Destroy camera object
		CLEyeDestroyCamera(_cam);
		// Destroy the allocated OpenCV image
		cvReleaseImage(&pCapImage);
		_cam = NULL;
}

DWORD WINAPI CLEyeCameraCapture::CaptureThread(LPVOID instance)
{
	// seed the rng with current tick count and thread id
	srand(GetTickCount() + GetCurrentThreadId());
	// forward thread to Capture function
	CLEyeCameraCapture *pThis = (CLEyeCameraCapture *)instance;
	pThis->Run();
	return 0;
}


