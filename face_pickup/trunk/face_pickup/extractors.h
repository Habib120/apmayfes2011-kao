#import<iostream.h>

#pragma once

namespace extractors
{
	class Extractor
	{
	public:
		virtual void Extract(HeadData data, IplImage* result) = 0;
	}

	class ExtractorSchema : public Extractor
	{
	public:
		ExtractorSchema();
		ExtractorSchema(std::vector<Extractor> schema);
		void Add(Exractor extractor);
	}
}

namespace extractors::concrete
{
	class GaborExtractor : public Extractor
	{
	}
}