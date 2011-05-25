//network.h�͈�ԏ��߂ɃC���N���[�h����K�v������
#include "network.h"
#include "cv.h"
#include <cppunit/extensions/HelperMacros.h> // 

// �ȉ���GaborExtractorTest�N���X�̐錾-----
class ServerClientTest : public CPPUNIT_NS::TestFixture { // 
  CPPUNIT_TEST_SUITE( ServerClientTest ); // 
  CPPUNIT_TEST( test_Connection );
  CPPUNIT_TEST_SUITE_END();
public:
  void setUp();      // 
  void tearDown();   // 

protected:
	void test_Connection();
};

// �ȉ���CounterTest�N���X�̎���-----

//CPPUNIT_TEST_SUITE_REGISTRATION( ServerClientTest ); // 

// �e�e�X�g�E�P�[�X�̎��s���O�ɌĂ΂��
void ServerClientTest::setUp() {
}


// �e�e�X�g�E�P�[�X�̎��s����ɌĂ΂��
void ServerClientTest::tearDown() {
}

// ����ȍ~�̓e�X�g�E�P�[�X�̎������e

/**
 * �����ʂ̒��o�ƁAC#�R�[�h�Ƃ̌݊����e�X�g
 * �Ƃ肠�����덷��10���ȓ��Ɏ��܂��Ă���΂悵�Ƃ���
 */
void ServerClientTest::test_Connection()
{
	SocketServer server;
	server.Start();

	SocketClient client;
	client.Send("Hello");

	server.Stop();
	CPPUNIT_ASSERT(true);
}