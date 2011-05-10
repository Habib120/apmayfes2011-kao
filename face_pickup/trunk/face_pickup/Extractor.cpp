#include "cv.h"
#include "extractors.h"
#include "structure.h"

/**
 * �w�肳�ꂽ�f�[�^��������ʂ𒊏o����
 * params
 *	data   : ��f�[�^
 *	result : �o�͂ƂȂ�����x�N�g�����i�[����\����	
 */
void Extractor::Extract(HeadData data, CvMat *result)
{
	this->preExtract(data, result);
	this->doExtract(data, result);
	this->postExtract(data, result);
}

/**
 * ���o�O�ɍs�����������͂�����I�[�o�[���C�h
 * �f�t�H���g�ł̓T�C�Y�̃`�F�b�N���s��
 */
void Extractor::preExtract(HeadData data, CvMat *result)
{
	cv::Size size(cvGetSize(result));
	if (size.height != 1 || size.width != this->GetFeatureCount())
	{
		char *msg;
		sprintf(msg, "result should be 1x%d matrix, %dx%d given.", this->GetFeatureCount(), size.height, size.width);
		throw msg;
	}
}

/**
 * ���o�O�ɍs�����������͂�����I�[�o�[���C�h
 * �f�t�H���g�ł͉������Ȃ�
 */
void Extractor::postExtract(HeadData data, CvMat *result)
{
}