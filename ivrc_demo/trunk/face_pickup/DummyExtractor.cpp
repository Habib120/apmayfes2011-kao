#include "extractors.h"
#include "cv.h"

int DummyExtractor::GetFeatureCount()
{
	return 10;
}

void DummyExtractor::doExtract(HeadData data, CvMat *result)
{
	for (int i = 0; i < 10; i++)
	{
		result->data.fl[i] = 10;
	}
}