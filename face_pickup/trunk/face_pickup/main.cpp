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
	IplImage* image = cvLoadImage("moulin_photo01.jpg");
	FaceComDetector detector;
	std::vector<FaceComDetectionResult> results = detector.Detect(image);
	
	std::cout << std::endl << "GetMoulinPhotoのTestを開始します" << std::endl;
	cv::Mat imagem = image;
	cv::imshow("src", imagem);
	IplImage* photo = MoulinPhotoMaker::GetMoulinPhoto(image, results);
	cv::Mat photom = photo;
	cv::imshow("photo", photo);
	cv::waitKey(0);
	std::cout << "結果は正しいですか？" << std::endl;

	//HeadTracker tracker;
	//PersonDetectionLoop pl(&tracker);
	//FaceComDetectionLoop fl(&tracker);
	//tracker.Start();
	//pl.Start();
	//fl.Start();

	//cvNamedWindow("test");
	//cvWaitKey(0);

	//fl.Stop();
	//pl.Stop();
	//tracker.Stop();
	//cvDestroyAllWindows();
	//return 0;
}