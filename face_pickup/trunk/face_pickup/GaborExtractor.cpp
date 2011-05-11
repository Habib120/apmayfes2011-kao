#include "extractors.h"
#include "structure.h"

//static�t�B�[���h�͍Ē�`���Ȃ���΂Ȃ�Ȃ��B
std::vector<CvGabor> GaborExtractor::filters;

void GaborExtractor::doExtract(HeadData data, CvMat *result)
{
	if (filters.size() == 0)
	{
		initFilters();
	}

	/*
	CvMat *mat  = cvCreateMat(src->width, src->height, CV_32FC1); //�摜�Ɠ����T�C�Y�̍s��𐶐�
	CvMat *smat = cvCreateMat(20, 20, CV_32FC1); //�k����̍s��𐶐�
	CvMat *feat = cvCreateMat(1, size*40, CV_32FC1); //�����ʂ��i�[����x�N�g���𐶐�
	CvMat row_header, *row;
	CvMat tmp;
	*/

	int i;
	for (i = 0; i < filters.size(); i++)
	{
		/*
		gabor.get_value(src, mat); //Gabor�����ʂ�mat�Ɋi�[
		cvResize(mat, smat, CV_INTER_LINEAR); //�s����k��
		row = cvReshape(smat, &row_header, 0, 1); //�s����s�x�N�g���ɕό`
		//cout << CV_MAT_ELEM(*row, float, 0, size-1) << endl;
		cvGetCols(feat, &tmp, size*k, size*(k+1));
		cvCopy(row, &tmp); //feat�̈ꕔ�ɓ����ʂ���ׂ��s�x�N�g�����R�s�[
		*/
	}
}

void GaborExtractor::initFilters()
{
	int u, v;
	for (u = 0; u < 5; u++)
	{
		for (v = 0; v < 8; v++)
		{
			CvGabor filter(u, v);
			filters.push_back(filter);
		}
	}
}

