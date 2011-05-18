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
	HeadData data = HeadData::GetTestData();
	FaceComDetector detector;
	for (int i = 0; i < 10; i++)
	{
		FaceComDetectionResult result = detector.Detect(data);
		std::string msg = result.HasData() ? "detection success!" : "detection failure!";
		cout << msg << endl;
		cout << "is_smiling : " << result.is_smiling << " confidence : " << result.con_smiling << endl;
		cout << "is_male : "    << result.is_male    << " confidence : " << result.con_gender  << endl;
		cout << "has_glasses : "<< result.has_glasses<< " confidence : " << result.con_glasses << endl;
	}
}