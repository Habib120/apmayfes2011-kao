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


HANDLE mutex;
//HANDLE _hThread[2];
double cam_coord[3];
double coord[3];
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
		cvShowImage(_windowName, pCapImage);
		CaptureMat = pCapImage;
		Point = getviewpoint(CaptureMat);
		WaitForSingleObject(mutex, INFINITE);
	/*	switch(cam_num){
		case 0:
			coord[0] = Point.x;
			coord[1] = Point.y;
		case 1:
			coord[2] = Point.x;
		}*/
		if(cam_num == 0){
			cam_coord[0] = Point.x;
			cam_coord[1] = Point.y;
		}else if(cam_num == 1){
			cam_coord[2] = Point.x;
		}else{
		}
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


