#include <cv.h>
#include "structure.h"

#pragma once

class ImageUtils
{
public:
	static IplImage* clipHeadImage(IplImage* src, HeadPose pose);
	static void rotateImage( IplImage *target, double angle, cv::Point2f center);
};