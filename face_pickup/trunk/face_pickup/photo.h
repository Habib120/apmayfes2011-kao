#include "detectors.h"
#include "structure.h"

#pragma once 

class MoulinPhotoMaker
{
public:
	static IplImage* GetMoulinPhoto(IplImage* image, std::vector<FaceComDetectionResult> results);
};