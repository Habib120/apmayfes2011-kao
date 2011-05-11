#include<iostream>
#include "structure.h"
#include "cvgabor.h"

#pragma once
class Extractor
{
public:
	virtual void Extract(HeadData data, CvMat* result);
	virtual int GetFeatureCount();
protected:
	virtual void doExtract(HeadData data, CvMat* result);
	virtual void preExtract(HeadData data, CvMat* result);
	virtual void postExtract(HeadData data, CvMat* result);
};

class ExtractorSchema : public Extractor
{
public:
	ExtractorSchema();
	virtual void Add(Extractor *extractor);
protected:
	typedef std::vector<Extractor*> ExtractorContainer;
	ExtractorContainer extractors;
	void doExtract(HeadData data, CvMat* result);
	void postExtract(HeadData data, CvMat* result);
	int GetFeatureCount();
};

class GaborExtractor : public Extractor
{
protected:
	void doExtract(HeadData data, CvMat* result);
	void postExtract(HeadData data, CvMat* result);
	int GetFeatureCount();
	static void initFilters();
	typedef std::vector<CvGabor> FilterContainer;
	static FilterContainer filters;
};