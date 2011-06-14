#include "extractors.h"
#include "structure.h"
#include "cv.h"

//static�t�B�[���h�͍Ē�`���Ȃ���΂Ȃ�Ȃ��B
std::vector<CvGabor *> GaborExtractor::filters;

void GaborExtractor::doExtract(HeadData data, CvMat *result)
{
	if (filters.size() == 0)
	{
		initFilters();
	}

	IplImage* raw_src = data.GetImage();
	IplImage* raw_src_gray = cvCreateImage(cvGetSize(raw_src), IPL_DEPTH_8U, 1);
	cvCvtColor(raw_src, raw_src_gray, CV_BGR2GRAY);
	
	IplImage* src = cvCreateImage(cvSize(in_width, in_width), IPL_DEPTH_8U, 1);
	cvResize(raw_src_gray, src, CV_INTER_LINEAR);
	CvMat *mat  = cvCreateMat(src->width, src->height, CV_32FC1); //�摜�Ɠ����T�C�Y�̍s��𐶐�
	CvMat *smat = cvCreateMat(out_width, out_width, CV_32FC1); //�k����̍s��𐶐�
	CvMat row_header, *row;
	CvMat tmp;
	int i;

	int size = out_width * out_width;
	for (i = 0; i < filters.size(); i++)
	{
		CvGabor* filter = filters.at(i);
		filter->get_value(src, mat); //Gabor�����ʂ�mat�Ɋi�[
		cvResize(mat, smat, CV_INTER_LINEAR); //�s����k��
		row = cvReshape(smat, &row_header, 0, 1); //�s����s�x�N�g���ɕό`
		cvGetCols(result, &tmp, size*i, size*(i + 1));
		cvCopy(row, &tmp); //feat�̈ꕔ�ɓ����ʂ���ׂ��s�x�N�g�����R�s�[
	}

	cvReleaseImage(&raw_src_gray);
	cvReleaseMat(&mat);
	cvReleaseMat(&smat);
	cvReleaseImage(&src);
}

void GaborExtractor::initFilters()
{
	int u, v;
	for (u = 0; u < 5; u++)
	{
		for (v = 0; v < 8; v++)
		{
			CvGabor *filter = new CvGabor(u, v);
			filters.push_back(filter);
		}
	}
}

int GaborExtractor::GetFeatureCount()
{
	return 40 * out_width * out_width;
}

int GaborExtractor::getInputWidth()
{
	return in_width;
}

int GaborExtractor::getOutputWidth()
{
	return out_width;
}

