#include "cv.h"
#include "ml.h"
#include "extractors.h"
#include "structure.h"

#pragma once

class Detector
{
public:
	Detector() :action(0) {}
	virtual std::string Detect(HeadData data);
	virtual void SetDetectAction(void (*action)(std::string));
protected:
	void (*action)(std::string);
	virtual std::string doDetect(HeadData data);
	virtual void preDetect(HeadData data);
	virtual std::string postDetect(HeadData data, std::string result);
};

class SmileDetector : public Detector
{
protected:
	static CvERTrees ert;
	static ExtractorSchema *es;
	virtual void preDetect(HeadData data);
	virtual std::string doDetect(HeadData data);
};
