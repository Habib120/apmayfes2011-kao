#include "cv.h"
#include "ml.h"
#include "extractors.h"
#include "structure.h"

#pragma once

class SmileDetector
{
public:
	virtual std::string Detect(HeadData data);
protected:
	static CvERTrees ert;
	static ExtractorSchema *es;
};
