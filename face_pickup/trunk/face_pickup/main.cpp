//#define _CRTDBG_MAP_ALLOC

//network.hを一番初めにインクルードすること
#include "network.h"
#include "detectors.h"
#include <stdio.h>
#include <cv.h>
#include <highgui.h>
#include <ctype.h>
#include <iostream>
#include <conio.h>
#include "sm_api.h"
#include <cmath>
#include <time.h>
#include <ml.h>
#include "cvgabor.h"
#include "structure.h"
#include "extractors.h"
#include "detectors.h"
#include "util.h"
#include "tracker.h"
#include "detection_thread.h"

#include <boost/thread.hpp>

//#include <stdlib.h>
//#include <crtdbg.h>

using std::cout;
using std::endl;
using std::cerr;
using std::cin;
using std::string;
//using namespace sm::faceapi;	

int main ()
{
<<<<<<< .mine
	/*
	SocketServer server;
	server.Start();
	for (int i = 0; i < 1000; i++)
	{
		std::cout << "client" << i << std::endl;
		SocketClient client;
		client.Send("rotate 1");
		Sleep(60);
	}
	server.Stop();
	*/
	//char c;
	//std::cin >> c;

	cvNamedWindow("Capture");
	HeadTracker tracker;
	SmileDetectionLoop sloop(&tracker);
	tracker.Start(true);
	sloop.Start();
	cvWaitKey(0);
	tracker.Stop();
	sloop.Stop();
	cvDestroyAllWindows();
	return 0;
=======
	HeadTracker tracker;
	FaceComDetectionLoop facecom(&tracker);

	tracker.Start(true);
	facecom.Start();

	cvNamedWindow("test");
	cvWaitKey(0);
	facecom.Stop();
	tracker.Stop();
>>>>>>> .r44
}