#include<iostream>
#include "structure.h"
#include "cvgabor.h"

#pragma once
class Extractor
{
public:
	virtual void Extract(HeadData data, CvMat* result);
	virtual int GetFeatureCount() = 0;
protected:
	virtual void doExtract(HeadData data, CvMat* result) = 0;
	virtual void preExtract(HeadData data, CvMat* result);
	virtual void postExtract(HeadData data, CvMat* result);
};

class ExtractorSchema : public Extractor
{
public:
	virtual void Add(Extractor *extractor);
	int GetFeatureCount();
protected:
	typedef std::vector<Extractor*> ExtractorContainer;
	ExtractorContainer extractors;
	void doExtract(HeadData data, CvMat* result);
};

typedef std::vector<CvGabor *> FilterContainer;
class GaborExtractor : public Extractor
{
public:
	int GetFeatureCount();
	static int getInputWidth();
	static int getOutputWidth();
protected:
	void doExtract(HeadData data, CvMat* result);
	static void initFilters();
	static FilterContainer filters;
	static const int in_width = 128;
	static const int out_width = 20;
};

class DummyExtractor : public Extractor
{
public:
	int GetFeatureCount();
protected:
	void doExtract(HeadData data, CvMat* result);
};