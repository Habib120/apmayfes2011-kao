#include "extractors.h"
#include "structure.h"

//staticフィールドは再定義しなければならない。
std::vector<CvGabor> GaborExtractor::filters;

void GaborExtractor::doExtract(HeadData data, CvMat *result)
{
	if (filters.size() == 0)
	{
		initFilters();
	}

	/*
	CvMat *mat  = cvCreateMat(src->width, src->height, CV_32FC1); //画像と同じサイズの行列を生成
	CvMat *smat = cvCreateMat(20, 20, CV_32FC1); //縮小先の行列を生成
	CvMat *feat = cvCreateMat(1, size*40, CV_32FC1); //特徴量を格納するベクトルを生成
	CvMat row_header, *row;
	CvMat tmp;
	*/

	int i;
	for (i = 0; i < filters.size(); i++)
	{
		/*
		gabor.get_value(src, mat); //Gabor特徴量をmatに格納
		cvResize(mat, smat, CV_INTER_LINEAR); //行列を縮小
		row = cvReshape(smat, &row_header, 0, 1); //行列を行ベクトルに変形
		//cout << CV_MAT_ELEM(*row, float, 0, size-1) << endl;
		cvGetCols(feat, &tmp, size*k, size*(k+1));
		cvCopy(row, &tmp); //featの一部に特徴量を並べた行ベクトルをコピー
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

