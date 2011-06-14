#include <cv.h>
#include "structure.h"

#pragma once

class ImageUtils
{
public:
	static IplImage* clipHeadImage(IplImage* src, HeadPose pose);
	static void rotateImage( IplImage *target, double angle, cv::Point2f center);
	static void adjustGamma( IplImage *src, IplImage* dst, double gamma);
	static void adjustBrightnessAndContrast(IplImage* src, IplImage* dst, int bparm, int cparam);
	static void adjustSaturation(IplImage* src, IplImage* dst, unsigned char sparam);
	static bool drawAlphaImage(IplImage* bgImg, IplImage* rgbImg, IplImage* alphaImg, int x, int y, int width, int height);
};

class FileUtils
{
public:
	static void DeleteAllTmpImages();
};