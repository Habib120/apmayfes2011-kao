//network.hを一番初めにインクルードする必要がある
#pragma once


#include "network.h"
#include <windows.h>
#include "structure.h"
#include "tracker.h"
#include <boost/thread/thread.hpp>
#include <boost/thread/mutex.hpp>

#include "targetver.h"

#include <cv.h>
#include <highgui.h>
#include <math.h>
#include <iostream>
#include <vector>

#include "CLEyeMulticam.h"

#define _USE_MATH_DEFINES

#include "getviewpoint.h"
#include "util.h"


#define THETA M_PI/6;

using std::cout;
using std::endl;
using std::vector;


class PersonDetectionLoop
{
public :
	PersonDetectionLoop(HeadTracker *t) : tracker(t), stop(false), person_confidence(0)
	{
	}
	void Start();
	void Stop();
	void operator()();
	void ResetState();
protected :
	PersonDetectionLoop() {}
	HeadTracker *tracker;
	boost::thread *loop_thread;
	bool stop;
	double person_confidence;
	static const int RUN_AVG_NUM = 70;
};

class CLEyeCameraCapture
{
protected:
	CHAR _windowName[256];
	GUID _cameraGUID;
	CLEyeCameraInstance _cam;
	CLEyeCameraColorMode _mode;
	CLEyeCameraResolution _resolution;
	float _fps;
	HANDLE _hThread;
	bool _running;
public:
	bool StartCapture();
	void StopCapture();
	void Run();
	static DWORD WINAPI CaptureThread(LPVOID instance);
	CLEyeCameraCapture(LPSTR windowName, GUID cameraGUID, CLEyeCameraColorMode mode, CLEyeCameraResolution resolution, float fps);
	int cam_num;
	cv::Mat CaptureMat;
	cv::Point2d Point;
};

class InfraredPersonDetectionLoop
{
protected:
	CLEyeCameraCapture **cam;
	boost::thread *loop_thread;
	bool stop;
	int numCams;
public:
	InfraredPersonDetectionLoop();
	void Start();
	void Stop();
	void operator()();

};

class BookObserver
{
protected:
	SerialAdapter *serial;
	boost::thread *loop_thread;
	bool stop;
	double cPage;
	void processReceivedText(std::string rcv, SocketClient *client);
	std::vector<int> getOpenPages(std::vector<int> sensor_values);
public:
	BookObserver();
	void Start();
	void Stop();
	void operator()();
};