#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "extractors.h"
#include "cv.h"

// 以下はGaborExtractorTestクラスの宣言-----
class GaborExtractorTest : public CPPUNIT_NS::TestFixture { // 
  CPPUNIT_TEST_SUITE( GaborExtractorTest ); // 
  CPPUNIT_TEST( test_Extract );
  CPPUNIT_TEST( test_Initialize );
  CPPUNIT_TEST_SUITE_END();          // 

protected:

public:
  void setUp();      // 
  void tearDown();   // 

protected:
  HeadData *data;
  void test_Extract();
  void test_Initialize();
};

// 以下はCounterTestクラスの実装-----

CPPUNIT_TEST_SUITE_REGISTRATION( GaborExtractorTest ); // 

// 各テスト・ケースの実行直前に呼ばれる
void GaborExtractorTest::setUp() {
	data = new HeadData();
	IplImage *img = cvLoadImage("face_0000.jpg");
	data->SetImage(img);
	data->pose = HeadPose();
}

// 各テスト・ケースの実行直後に呼ばれる
void GaborExtractorTest::tearDown() {
	delete data;
}

// これ以降はテスト・ケースの実装内容

void GaborExtractorTest::test_Extract()
{
	GaborExtractor *extractor = new GaborExtractor();
	CvMat* mat = cvCreateMat(1, extractor->GetFeatureCount(), CV_32FC1);
	cvSetZero(mat);
	extractor->Extract(*data, mat);
	CPPUNIT_ASSERT(mat->data.fl != 0);
	delete extractor;
}

void GaborExtractorTest::test_Initialize()
{
	CPPUNIT_ASSERT(true);
}