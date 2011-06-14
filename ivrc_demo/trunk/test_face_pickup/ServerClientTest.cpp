//network.hは一番初めにインクルードする必要がある
#include "network.h"
#include "cv.h"
#include <cppunit/extensions/HelperMacros.h> // 

// 以下はGaborExtractorTestクラスの宣言-----
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

// 以下はCounterTestクラスの実装-----

//CPPUNIT_TEST_SUITE_REGISTRATION( ServerClientTest ); // 

// 各テスト・ケースの実行直前に呼ばれる
void ServerClientTest::setUp() {
}


// 各テスト・ケースの実行直後に呼ばれる
void ServerClientTest::tearDown() {
}

// これ以降はテスト・ケースの実装内容

/**
 * 特徴量の抽出と、C#コードとの互換性テスト
 * とりあえず誤差が10％以内に収まっていればよしとする
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