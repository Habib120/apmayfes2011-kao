#include "photo.h"
#include "util.h"
#include "windows.h"
#include <algorithm>
using namespace std;

IplImage* MoulinPhotoMaker::GetMoulinPhoto(IplImage *camimage, std::vector<FaceComDetectionResult> results)
{
	IplImage* tmp = cvCreateImage(cvSize(camimage->width, camimage->height), camimage->depth, camimage->nChannels);
	IplImage* ret = cvCreateImage(cvSize(camimage->width, camimage->height), camimage->depth, camimage->nChannels);
	ImageUtils::adjustBrightnessAndContrast(camimage, tmp, 55, 40);
	ImageUtils::adjustSaturation(tmp, ret, 0);
	IplImage* mask = cvLoadImage("mask.png");
	IplImage* mask_alpha = cvLoadImage("maskA.png");

	const int margin = 30;
	CvRect rect = cvRect(0, 0, 296, 200);
	std::vector<CvRect> rects;
	int minx = camimage->width;
	int maxx = 0;
	int cx = 0;
	int cy = 0;
	for (int i = 0; i < results.size(); i++)
	{
		CvRect r = results.at(i).face_rect;
		if (minx > r.x)
		{
			minx = r.x;
		}
		else if (maxx < r.x + rect.width)
		{
			maxx = r.x + rect.width;
		}
		cx += (r.x + r.width) / results.size();
		cy += (r.y + r.height) / results.size();
	}
	if (minx < (camimage->width - maxx))
	{
		int candidate_1 = camimage->width - rect.width;
		int candidate_2 = maxx + margin;
		rect.x = min(candidate_1, candidate_2);
	}
	else
	{
		int candidate_1 = 0;
		int candidate_2 = minx - margin - rect.width;
		rect.x = max(candidate_1, candidate_2);
	}
	rect.y = max(0, cy);

	ImageUtils::drawAlphaImage(ret, mask, mask_alpha, rect.x, rect.y, rect.width, rect.height);

	cvReleaseImage(&mask);
	cvReleaseImage(&mask_alpha);

	return ret;
}

