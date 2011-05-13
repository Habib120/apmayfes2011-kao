//#define _CRTDBG_MAP_ALLOC

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
#include "faceapi.h"

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
	cvNamedWindow("Capture");
	HeadTracker tracker;
	tracker.Start(false);
	for (int i = 0; i < 100; i++)
	{
		HeadData *data = tracker.GetCurrentHeadData();
		if (data != 0)
		{
			data->ReleaseImage();
			delete data;
		}
		Sleep(30);
	}
	cvWaitKey(0);
	tracker.Stop();
	cvDestroyAllWindows();
	return 0;
}