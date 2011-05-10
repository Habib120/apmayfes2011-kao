#include <cv.h>
#include "structure.h"

#pragma once

namespace util
{
	class ImageUtils
	{
	public:
		static IplImage* clipHeadImage(IplImage* src, structure::HeadPose pose);
		static void rotateImage( IplImage *target, double angle, cv::Point2f center);
	};
};