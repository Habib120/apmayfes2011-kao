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
#include "photo.h"
#include "detection_thread.h"
#include "connection.h"

#include <boost/thread/thread.hpp>

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
	HeadTracker tracker;
	tracker.Start();

	PersonDetectionLoop pl(&tracker);
	FaceComDetectionLoop fl(&tracker);
	SocketServer server(&tracker, &pl);
	tracker.Start();
	pl.Start();
	fl.Start();
	server.Start();

	cvNamedWindow("test");
	cvWaitKey(0);

	//fl.Stop();
	//pl.Stop();
	//tracker.Stop();
	//cvDestroyAllWindows();
	//return 0;
}