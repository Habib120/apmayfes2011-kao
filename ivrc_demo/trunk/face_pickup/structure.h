#include <cv.h>

#pragma once

class HeadPose
{
public:
	//CvRect GetHeadAreaInCam();
	double x, y, z;
	double rx, ry, rz;
	double rot;
	bool isValueSet() const;
};

class HeadData
{
public:
	HeadData() : image(0) {}
	IplImage* GetImage();
	void SetImage(IplImage* image);
	void ReleaseImage();
	HeadPose pose;
	static HeadData GetTestData();
protected:
	IplImage* image;
};
