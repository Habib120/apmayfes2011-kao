#include <cv.h>

#pragma once

namespace structure
{
	class HeadPose
	{
	public:
		//CvRect GetHeadAreaInCam();
		double x, y, z;
		double rot;
		bool isValueSet() const;
	};

	class HeadData
	{
	public:
		IplImage* GetImage();
		void SetImage(IplImage* image);
		HeadPose pose;
	};
}
