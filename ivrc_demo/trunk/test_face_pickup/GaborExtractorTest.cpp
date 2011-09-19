#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "extractors.h"
#include "cv.h"
#include "tokenizer.h"

// �ȉ���GaborExtractorTest�N���X�̐錾-----
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
  static const int LOAD_LENGTH = 10;
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
	for(ifs >> token; token != "" && cnt < LOAD_LENGTH; ifs >> token) 
	{
		CV_MAT_ELEM(*ret, float, 0, cnt) = atof(token.data());
		cnt++;
	}

	return ret;
}


// �ȉ���CounterTest�N���X�̎���-----

CPPUNIT_TEST_SUITE_REGISTRATION( GaborExtractorTest ); // 

// �e�e�X�g�E�P�[�X�̎��s���O�ɌĂ΂��
void GaborExtractorTest::setUp() {
	data = new HeadData();
	IplImage *img = cvLoadImage("face_0000.jpg");
	data->SetImage(img);
	data->pose = HeadPose();

	//������p��
	answer = loadGaborFeaturePoints();
}


// �e�e�X�g�E�P�[�X�̎��s����ɌĂ΂��
void GaborExtractorTest::tearDown() {
	cvReleaseMat(&answer);
	delete data;
}

// ����ȍ~�̓e�X�g�E�P�[�X�̎������e

/**
 * �����ʂ̒��o�ƁAC#�R�[�h�Ƃ̌݊����e�X�g
 * �Ƃ肠�����덷��10���ȓ��Ɏ��܂��Ă���΂悵�Ƃ���
 */
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
		float actual = CV_MAT_ELEM(*mat, float, 0, i);
		float expected = CV_MAT_ELEM(*answer, float, 0, i);
		if (abs(actual - expected) > 0.20 * expected)
		{
			std::cerr << "not equal! actual : " << actual << " expected: " << expected << " dif : " << actual - expected << std::endl;
		}
		CPPUNIT_ASSERT(abs(actual - expected) < 0.20 * expected);
	}
	delete extractor;
}