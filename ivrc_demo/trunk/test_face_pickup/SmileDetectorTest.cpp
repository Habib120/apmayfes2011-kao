#include "detectors.h"
#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "cv.h"

// �ȉ���SmileDetectorTest�N���X�̐錾-----
class SmileDetectorTest : public CPPUNIT_NS::TestFixture { // 
  CPPUNIT_TEST_SUITE( SmileDetectorTest ); // 
  CPPUNIT_TEST( test_Detect );
  CPPUNIT_TEST_SUITE_END();
public:
  void setUp();      // 
  void tearDown();   // 

protected:
  HeadData *data;
  void test_Detect();
};

// �ȉ���CounterTest�N���X�̎���-----

CPPUNIT_TEST_SUITE_REGISTRATION( SmileDetectorTest ); // 

// �e�e�X�g�E�P�[�X�̎��s���O�ɌĂ΂��
void SmileDetectorTest::setUp() {
	data = new HeadData();
	IplImage *img = cvLoadImage("face_0000.jpg");
	data->SetImage(img);
	data->pose = HeadPose();
}


// �e�e�X�g�E�P�[�X�̎��s����ɌĂ΂��
void SmileDetectorTest::tearDown() {
	delete data;
}

// ����ȍ~�̓e�X�g�E�P�[�X�̎������e

/**
 *  
 * 
 */
void SmileDetectorTest::test_Detect()
{
	SmileDetector *detector = new SmileDetector();
	std::string result = detector->Detect(*data);
	CPPUNIT_ASSERT(result == "smiling");
	delete detector;
}