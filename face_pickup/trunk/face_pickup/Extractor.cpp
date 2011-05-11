#include "cv.h"
#include "extractors.h"
#include "structure.h"

/**
 * 指定されたデータから特徴量を抽出する
 * params
 *	data   : 顔データ
 *	result : 出力となる特徴ベクトルを格納する構造体	
 */
void Extractor::Extract(HeadData data, CvMat *result)
{
	this->preExtract(data, result);
	this->doExtract(data, result);
	this->postExtract(data, result);
}

/**
 * 抽出前に行いたい処理はこれをオーバーライド
 * デフォルトではサイズのチェックを行う
 */
void Extractor::preExtract(HeadData data, CvMat *result)
{
	cv::Size size(cvGetSize(result));
	if (size.height != 1 || size.width != this->GetFeatureCount())
	{
		char *msg;
		sprintf(msg, "Invalid Argument : result should be 1x%d matrix, %dx%d given.", this->GetFeatureCount(), size.height, size.width);
		throw msg;
	}
	if (data.GetImage()->depth != IPL_DEPTH_8U || data.GetImage()->nChannels != 3)
	{
		throw "Invalid Argument : HeadData image must contains 3channel BGR byte image";
	}
}

/**
 * 抽出前に行いたい処理はこれをオーバーライド
 * デフォルトでは何もしない
 */
void Extractor::postExtract(HeadData data, CvMat *result)
{
}