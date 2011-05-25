#include "photo.h"
#include "util.h"

IplImage* MoulinPhotoMaker::GetMoulinPhoto(IplImage *camimage, std::vector<FaceComDetectionResult> results)
{
	IplImage* ret = cvCreateImage(cvSize(camimage->width, camimage->height), camimage->depth, camimage->nChannels);
	cvCopy(camimage, ret);
	IplImage* mask = cvLoadImage("mask.png");
	IplImage* mask_alpha = cvLoadImage("maskA.png");

	CvRect rect = cvRect(0, 0, 200, 150);
	ImageUtils::drawAlphaImage(ret, mask, mask_alpha, rect.x, rect.y, rect.width, rect.height);

	cvReleaseImage(&mask);
	cvReleaseImage(&mask_alpha);

	return ret;
}

