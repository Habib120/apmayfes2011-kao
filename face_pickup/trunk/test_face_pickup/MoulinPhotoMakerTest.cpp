
#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "detectors.h"
#include "photo.h"
#include "cv.h"
#include "highgui.h"

// 以下はMoulinPhotoMakerTestクラスの宣言-----
class MoulinPhotoMakerTest : public CPPUNIT_NS::TestFixture { // 
  CPPUNIT_TEST_SUITE( MoulinPhotoMakerTest ); // 
  CPPUNIT_TEST( test_GetMoulinPhoto );
  CPPUNIT_TEST_SUITE_END();          // 

protected:

public:
  void setUp();      // 
  void tearDown();   // 

protected:
  void test_GetMoulinPhoto();
};

// 以下はCounterTestクラスの実装-----

CPPUNIT_TEST_SUITE_REGISTRATION( MoulinPhotoMakerTest ); // 

// 各テスト・ケースの実行直前に呼ばれる
void MoulinPhotoMakerTest::setUp() {
}

// 各テスト・ケースの実行直後に呼ばれる
void MoulinPhotoMakerTest::tearDown() {
}

// これ以降はテスト・ケースの実装内容

void MoulinPhotoMakerTest::test_GetMoulinPhoto()
{
	IplImage* image = cvLoadImage("moulin_photo01.jpg");
	FaceComDetector detector;
	std::vector<FaceComDetectionResult> results = detector.Detect(image);
	
	std::cout << std::endl << "GetMoulinPhotoのTestを開始します" << std::endl;
	cv::Mat imagem = image;
	cv::imshow("src", imagem);
	IplImage* photo = MoulinPhotoMaker::GetMoulinPhoto(image, results);
	cv::Mat photom = photo;
	cv::imshow("photo", photo);
	cv::waitKey(0);
	std::cout << "結果は正しいですか？" << std::endl;

	char r;
	std::cin >> r;
	cvDestroyAllWindows();
	cvReleaseImage(&photo);
	cvReleaseImage(&photo);

}