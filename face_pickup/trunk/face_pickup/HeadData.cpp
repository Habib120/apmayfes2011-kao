#include "cv.h"
#include "highgui.h"
#include "structure.h"


void HeadData::SetImage(IplImage *image)
{
	this->image = image;
}

IplImage* HeadData::GetImage()
{
	return this->image;
}

void HeadData::ReleaseImage()
{
	cvReleaseImage(&(this->image));
}

HeadData HeadData::GetTestData()
{
	HeadData data;
	IplImage *img = cvLoadImage("face_0000.jpg");
	data.SetImage(img);
	data.pose = HeadPose();
	return data;
}