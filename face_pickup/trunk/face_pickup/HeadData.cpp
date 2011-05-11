#include "cv.h"
#include "structure.h"


void HeadData::SetImage(IplImage *image)
{
	this->image = image;
}

IplImage* HeadData::GetImage()
{
	return this->image;
}