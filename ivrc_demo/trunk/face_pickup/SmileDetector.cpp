#include "detectors.h"
#include "cv.h"

std::string SmileDetector::Detect(HeadData data)
{
	//ランダムツリーの決定木をxmlファイルから初期化する
	if (ert.get_tree_count() == 0)
	{
		ert.load("rtrees.xml");
	}
	//使用する特徴量を設定
	if (es == 0)
	{
		es = new ExtractorSchema();
		es->Add(new GaborExtractor()); //取りあえずガボール特徴量
	}

	CvMat* feature = es->CreateFeatureMat();
	es->Extract(data, feature);
	double r = ert.predict(feature);
	return r == 0 ? "not_smiling" : "smiling";
	cvReleaseMat(&feature);
}


CvERTrees SmileDetector::ert; //static変数は外部で初期化する必要がある
ExtractorSchema *SmileDetector::es = 0;