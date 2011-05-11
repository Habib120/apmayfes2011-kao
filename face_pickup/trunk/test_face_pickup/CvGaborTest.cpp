#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
#include "cv.h"
#include "cvgabor.h"

// 以下はCvGaborTestクラスの宣言-----
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
	CvGabor* c;
	/*
	 * Defining mat
	 */
	cv::Mat src = cv::imread("face_0000.jpg"); //画像
	cv::Mat src_g(src.size(), CV_8UC1); //グレースケール変換したテスト画像
	IplImage ipl_src_g = src_g; //グレースケール画像のIplImage参照
	CvMat *mat = cvCreateMat(src.rows, src.cols, CV_32FC1);	//戻り値を受け取る配列

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
	std::cout << "結果は正しいですか？" << std::endl;
	char ans;
	std::cin >> ans;
	CPPUNIT_ASSERT(ans == 'y');
}