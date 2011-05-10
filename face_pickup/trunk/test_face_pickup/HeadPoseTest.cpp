#include <cppunit/extensions/HelperMacros.h> // 
#include "structure.h"
using namespace structure;

// �ȉ���HeadPoseTest�N���X�̐錾-----
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

// �ȉ���CounterTest�N���X�̎���-----

CPPUNIT_TEST_SUITE_REGISTRATION( HeadPoseTest ); // 

// �e�e�X�g�E�P�[�X�̎��s���O�ɌĂ΂��
void HeadPoseTest::setUp() {
  h = new HeadPose();
}

// �e�e�X�g�E�P�[�X�̎��s����ɌĂ΂��
void HeadPoseTest::tearDown() {
  delete h;
}

// ����ȍ~�̓e�X�g�E�P�[�X�̎������e

void HeadPoseTest::test_SetValue()
{
	h->x = 0;
	h->y = 0;
	h->z = 0;
	h->rot = 0;
	CPPUNIT_ASSERT(!h->isValueSet());
}