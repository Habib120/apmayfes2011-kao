#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
using namespace structure;

// 以下はHeadPoseTestクラスの宣言-----
class HeadPoseTest : public CPPUNIT_NS::TestFixture { // 
  CPPUNIT_TEST_SUITE( HeadPoseTest ); // 
  CPPUNIT_TEST( test_SetValue );
  CPPUNIT_TEST_SUITE_END();          // 

protected:
  HeadPose* h;

public:
  void setUp();      // 
  void tearDown();   // 

protected:
  void test_SetValue();
};

// 以下はCounterTestクラスの実装-----

CPPUNIT_TEST_SUITE_REGISTRATION( HeadPoseTest ); // 

// 各テスト・ケースの実行直前に呼ばれる
void HeadPoseTest::setUp() {
  h = new HeadPose();
}

// 各テスト・ケースの実行直後に呼ばれる
void HeadPoseTest::tearDown() {
  delete h;
}

// これ以降はテスト・ケースの実装内容

void HeadPoseTest::test_SetValue()
{
	h->x = 0;
	h->y = 0;
	h->z = 0;
	h->rot = 0;
	CPPUNIT_ASSERT(!h->isValueSet());
}