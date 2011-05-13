#include "detectors.h"
#include "cv.h"

std::string SmileDetector::Detect(HeadData data)
{
	//�����_���c���[�̌���؂�xml�t�@�C�����珉��������
	if (ert.get_tree_count() == 0)
	{
		ert.load("rtrees.xml");
	}
	//�g�p��������ʂ�ݒ�
	if (es == 0)
	{
		es = new ExtractorSchema();
		es->Add(new GaborExtractor()); //��肠�����K�{�[��������
	}

	CvMat* feature = es->CreateFeatureMat();
	es->Extract(data, feature);
	double r = ert.predict(feature);
	return r == 0 ? "not_smiling" : "smiling";
	cvReleaseMat(&feature);
}


CvERTrees SmileDetector::ert; //static�ϐ��͊O���ŏ���������K�v������
ExtractorSchema *SmileDetector::es = 0;