//#define _CRTDBG_MAP_ALLOC

//network.hを一番初めにインクルードすること
#include "network.h"
#include <stdio.h>
#include <cv.h>
#include <highgui.h>
#include <ctype.h>
#include <iostream>
#include <conio.h>
#include <cmath>
#include <time.h>
#include <ml.h>
#include "structure.h"
#include "util.h"
#include "detection_thread.h"

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
	

	InfraredPersonDetectionLoop ipl;
	BookObserver observer;
	ipl.Start();
	//observer.Start();

	cvNamedWindow("test");
	cvWaitKey(0);

	ipl.Stop();
	cvDestroyAllWindows();
	return 0;
}