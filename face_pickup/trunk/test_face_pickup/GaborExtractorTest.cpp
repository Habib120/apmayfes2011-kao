#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "extractors.h"
#include "cv.h"
#include "tokenizer.h"

// 以下はGaborExtractorTestクラスの宣言-----
class GaborExtractorTest : public CPPUNIT_NS::TestFixture { // 
  CPPUNIT_TEST_SUITE( GaborExtractorTest ); // 
  CPPUNIT_TEST( test_Extract );
  CPPUNIT_TEST_SUITE_END();
public:
  void setUp();      // 
  void tearDown();   // 

protected:
  HeadData *data;
  CvMat *answer;
  void test_Extract();
  static CvMat* loadGaborFeaturePoints(); 
  static const int LOAD_LENGTH = 20;
};


CvMat* GaborExtractorTest::loadGaborFeaturePoints()
{
	std::ifstream ifs("featurepoints_gabor.txt");
	std::string header, token;
	ifs >> header;
	int nfeatures = atoi(header.data());
	//
	CvMat* ret = cvCreateMat(1, nfeatures, CV_32FC1);
	cvSetZero(ret);
	int cnt = 0;
	for(ifs >> token; token.size() != 0 && cnt < LOAD_LENGTH; ifs >> token) 
	{
		CV_MAT_ELEM(*ret, float, 0, cnt) = atof(token.data());
		cnt++;
	}

	return ret;
}


// 以下はCounterTestクラスの実装-----

CPPUNIT_TEST_SUITE_REGISTRATION( GaborExtractorTest ); // 

// 各テスト・ケースの実行直前に呼ばれる
void GaborExtractorTest::setUp() {
	std::cout << std::endl;
	std::cout << "Loading test data...";
	data = new HeadData();
	IplImage *img = cvLoadImage("face_0000.jpg");
	data->SetImage(img);
	data->pose = HeadPose();

	//正解を用意
	answer = loadGaborFeaturePoints();
	std::cout << "done" << std::endl;
}


// 各テスト・ケースの実行直後に呼ばれる
void GaborExtractorTest::tearDown() {
	cvReleaseMat(&answer);
	delete data;
}

// これ以降はテスト・ケースの実装内容

void GaborExtractorTest::test_Extract()
{
	GaborExtractor *extractor = new GaborExtractor();
	CvMat* mat = cvCreateMat(1, extractor->GetFeatureCount(), CV_32FC1);
	cvSetZero(mat);
	extractor->Extract(*data, mat);
	CPPUNIT_ASSERT(*(mat->data.fl) != 0);
	CPPUNIT_ASSERT(mat->width == answer->width);
	for (int i = 0; i < LOAD_LENGTH; i++)
	{
		if (CV_MAT_ELEM(*mat, float, 0, i) != CV_MAT_ELEM(*answer, float, 0, i))
		{
			std::cerr << "not equal! calculated : " << CV_MAT_ELEM(*mat, float, 0, i) << " answer: " << CV_MAT_ELEM(*answer, float, 0, i) << std::endl;
		}
		CPPUNIT_ASSERT(answer->data.fl[i] == mat->data.fl[i]);
	}
	delete extractor;
}