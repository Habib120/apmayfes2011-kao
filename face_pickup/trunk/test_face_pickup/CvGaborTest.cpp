#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "cv.h"
#include "cvgabor.h"

// �ȉ���CvGaborTest�N���X�̐錾-----
class CvGaborTest : public CPPUNIT_NS::TestFixture { // 
  CPPUNIT_TEST_SUITE( CvGaborTest ); // 
  //CPPUNIT_TEST( test_Kernel );
  CPPUNIT_TEST( test_Convert );
  CPPUNIT_TEST_SUITE_END();          // 

protected:

public:
  void setUp();      // 
  void tearDown();   // 

protected:
  void test_Kernel();
  void test_Convert();
};

// �ȉ���CounterTest�N���X�̎���-----

CPPUNIT_TEST_SUITE_REGISTRATION( CvGaborTest ); // 

// �e�e�X�g�E�P�[�X�̎��s���O�ɌĂ΂��
void CvGaborTest::setUp() {
}

// �e�e�X�g�E�P�[�X�̎��s����ɌĂ΂��
void CvGaborTest::tearDown() {
}

// ����ȍ~�̓e�X�g�E�P�[�X�̎������e

//�J�[�l����������Ɛ�������Ă��邩�e�X�g����
void CvGaborTest::test_Kernel()
{
	int u, v;
	cv::namedWindow("test");
	for (u = 0; u < 5; u++)
	{
		for (v = 0; v < 5; v++)
		{
			CvGabor *c = new CvGabor(u, v);
			cv::imshow("test", c->getKernelImage(CV_GABOR_REAL));
			cv::waitKey(0);
			delete c;
		}
	}
	cvDestroyAllWindows();
	std::cout << "���ʂ͐������ł����H" << std::endl;
	char ans;
	std::cin >> ans;
	CPPUNIT_ASSERT(ans == 'y');
}

/**
 * �ϊ���(���o�I�Ɍ���)�������s���Ă��邩
 */
void CvGaborTest::test_Convert()
{
	CvGabor* c;
	/*
	 * Defining mat
	 */
	cv::Mat src = cv::imread("face_0000.jpg"); //�摜
	cv::Mat src_g(src.size(), CV_8UC1); //�O���[�X�P�[���ϊ������e�X�g�摜
	IplImage ipl_src_g = src_g; //�O���[�X�P�[���摜��IplImage�Q��
	CvMat *mat = cvCreateMat(src.rows, src.cols, CV_32FC1);	//�߂�l���󂯎��z��

	/*
	 * Converting data to mat
	 */
	cv::cvtColor(src, src_g, CV_BGR2GRAY);

	cv::imshow("base_img", src);
	cv::waitKey(0);

	int u, v;
	for (u = 0; u < 5; u++)
	{
		for (v = 0; v < 8; v++)
		{
			c = new CvGabor(u, v);
			c->get_value(&ipl_src_g, mat);
			cv::Mat dispmat = mat;
			cv::imshow("conv", c->convertToByte(mat, 3));
			cv::waitKey(0);
		}
	}

	cvDestroyAllWindows();
	std::cout << "���ʂ͐������ł����H" << std::endl;
	char ans;
	std::cin >> ans;
	CPPUNIT_ASSERT(ans == 'y');
}