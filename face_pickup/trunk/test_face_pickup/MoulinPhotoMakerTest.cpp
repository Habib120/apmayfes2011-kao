
#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "detectors.h"
#include "photo.h"
#include "cv.h"
#include "highgui.h"

// �ȉ���MoulinPhotoMakerTest�N���X�̐錾-----
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

// �ȉ���CounterTest�N���X�̎���-----

CPPUNIT_TEST_SUITE_REGISTRATION( MoulinPhotoMakerTest ); // 

// �e�e�X�g�E�P�[�X�̎��s���O�ɌĂ΂��
void MoulinPhotoMakerTest::setUp() {
}

// �e�e�X�g�E�P�[�X�̎��s����ɌĂ΂��
void MoulinPhotoMakerTest::tearDown() {
}

// ����ȍ~�̓e�X�g�E�P�[�X�̎������e

void MoulinPhotoMakerTest::test_GetMoulinPhoto()
{
	IplImage* image = cvLoadImage("moulin_photo01.jpg");
	FaceComDetector detector;
	std::vector<FaceComDetectionResult> results = detector.Detect(image);
	
	std::cout << std::endl << "GetMoulinPhoto��Test���J�n���܂�" << std::endl;
	cv::Mat imagem = image;
	cv::imshow("src", imagem);
	IplImage* photo = MoulinPhotoMaker::GetMoulinPhoto(image, results);
	cv::Mat photom = photo;
	cv::imshow("photo", photo);
	cv::waitKey(0);
	std::cout << "���ʂ͐������ł����H" << std::endl;

	char r;
	std::cin >> r;
	cvDestroyAllWindows();
	cvReleaseImage(&photo);
	cvReleaseImage(&photo);

}