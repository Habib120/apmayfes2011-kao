#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "cv.h"
#include "cvgabor.h"

// 以下はCvGaborTestクラスの宣言-----
class CvGaborTest : public CPPUNIT_NS::TestFixture { // 
  CPPUNIT_TEST_SUITE( CvGaborTest ); // 
  CPPUNIT_TEST( test_Kernel );
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

// 以下はCounterTestクラスの実装-----

CPPUNIT_TEST_SUITE_REGISTRATION( CvGaborTest ); // 

// 各テスト・ケースの実行直前に呼ばれる
void CvGaborTest::setUp() {
}

// 各テスト・ケースの実行直後に呼ばれる
void CvGaborTest::tearDown() {
}

// これ以降はテスト・ケースの実装内容

//カーネルがきちんと生成されているかテストする
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
	std::cout << "結果は正しいですか？" << std::endl;
	char ans;
	std::cin >> ans;
	CPPUNIT_ASSERT(ans == 'y');
}

/**
 * 変換が(視覚的に見て)正しく行われているか
 */
void CvGaborTest::test_Convert()
{
	CvGabor *c = new CvGabor(2, 3);
	cv::Mat src = cv::imread("face_0000.jpg");
	cv::imshow("base_img", src);
	cv::Mat mat(src.size(), CV_32FC1);
	c->get_mat(src, mat);
	cv::imshow("conv_img", c->convertToByte(mat));
	cv::waitKey(0);

	CPPUNIT_ASSERT(true);
}