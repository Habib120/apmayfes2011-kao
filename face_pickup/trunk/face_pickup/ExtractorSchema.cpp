#include "structure.h"
#include "extractors.h"
#include "cv.h"


//���݂̔z���Extractor��ǉ�����
void ExtractorSchema::Add(Extractor *extractor)
{
	this->extractors.push_back(extractor);
}

//�����ʂ̑傫����Ԃ�
int ExtractorSchema::GetFeatureCount()
{
	int count = 0;
	int i = 0;
	for (; i < extractors.size(); i++)
	{
		count += extractors.at(i)->GetFeatureCount();
	}
	return count;
}

void ExtractorSchema::doExtract(HeadData data, CvMat *result)
{
	int i, current;
	current = 0;
	for (i = 0; i < extractors.size(); i++)
	{
		CvMat target;
		cvGetCols(result, &target, current, current + extractors.at(i)->GetFeatureCount());
		extractors.at(i)->Extract(data, &target);
		current += extractors.at(i)->GetFeatureCount();
	}
}