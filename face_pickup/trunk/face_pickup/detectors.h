#include "cv.h"
#include "ml.h"
#include "extractors.h"
#include "structure.h"
#include <iostream>

#pragma once

class SmileDetector
{
public:
	virtual std::string Detect(HeadData data);
protected:
	static CvERTrees ert;
	static ExtractorSchema *es;
};

class FaceComDetectionResult
{
public:
	bool is_smiling;
	bool has_glasses;
	bool is_male;
	double con_smiling;
	double con_glasses;
	double con_gender;
	bool has_data;
protected:
};

class FaceComDetector
{
public:
	virtual std::vector<FaceComDetectionResult> Detect(HeadData data);
	virtual std::vector<FaceComDetectionResult> Detect(IplImage* image);
protected:
	void initialize();
	static std::string host;
	static std::string header_str;
	static std::string footer_str;
	static std::string boundary;
	static std::string ipaddress;
	static size_t header_size;
	static size_t footer_size;
};

