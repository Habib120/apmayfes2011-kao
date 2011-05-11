#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "extractors.h"
#include "cv.h"

// �ȉ���GaborExtractorTest�N���X�̐錾-----
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

// �ȉ���CounterTest�N���X�̎���-----

CPPUNIT_TEST_SUITE_REGISTRATION( GaborExtractorTest ); // 

// �e�e�X�g�E�P�[�X�̎��s���O�ɌĂ΂��
void GaborExtractorTest::setUp() {
	data = new HeadData();
	IplImage *img = cvLoadImage("face_0000.jpg");
	data->SetImage(img);
	data->pose = HeadPose();
}

// �e�e�X�g�E�P�[�X�̎��s����ɌĂ΂��
void GaborExtractorTest::tearDown() {
	delete data;
}

// ����ȍ~�̓e�X�g�E�P�[�X�̎������e

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