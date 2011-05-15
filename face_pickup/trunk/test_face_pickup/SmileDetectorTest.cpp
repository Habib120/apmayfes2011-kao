#include "detectors.h"
#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "cv.h"

// 以下はSmileDetectorTestクラスの宣言-----
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

// 以下はCounterTestクラスの実装-----

CPPUNIT_TEST_SUITE_REGISTRATION( SmileDetectorTest ); // 

// 各テスト・ケースの実行直前に呼ばれる
void SmileDetectorTest::setUp() {
	data = new HeadData();
	IplImage *img = cvLoadImage("face_0000.jpg");
	data->SetImage(img);
	data->pose = HeadPose();
}


// 各テスト・ケースの実行直後に呼ばれる
void SmileDetectorTest::tearDown() {
	delete data;
}

// これ以降はテスト・ケースの実装内容

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