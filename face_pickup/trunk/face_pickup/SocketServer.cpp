#include <winsock2.h>
#include "connection.h"
#include <iostream>
#include <stdio.h>


void SocketServer::Start()
{
	int result = 0;
	//接続先情報初期化
	memset(&source, 0, sizeof(source));
	//アドレスファミリーはINET(共通)
	source.sin_family = AF_INET;
	//ポート番号はクライアントプログラムと共通
	source.sin_port = htons(SERVER_PORT);
	//どこからの接続要求も受け入れる
	source.sin_addr.s_addr = htonl(INADDR_ANY);

	//ソケット通信の開始準備
	WSADATA data;	
	result = WSAStartup(MAKEWORD(2, 0), &data);
	if (result < 0){
		printf("%d\n", GetLastError());
		printf("[server]ソケット通信準備エラー\n");
	}

	//ソケットの生成
	s = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (s < 0){
		printf("%d\n", GetLastError());
		printf("[server]ソケット生成エラー\n");
	}

	//ソケットのバインド
	result = bind(s, (struct sockaddr *)&source, sizeof(source));
	if (result < 0){
		printf("%d\n", GetLastError());
		printf("[server]バインドエラー\n");
	}

	//接続の許可
	result = listen(s, 1);
	if (result < 0){
		printf("[server]接続許可エラー\n");
	}
	printf("[server]接続開始\n");
	loop_thread = new boost::thread(boost::ref(*this));
	running = true;
}

void SocketServer::Stop()
{
	if (!running)
	{
		return;
	}
	running = false;
	loop_thread->join();
	delete loop_thread;
	//ソケット通信の終了
	WSACleanup();
}

void SocketServer::operator()()
{
	//受信データのバッファ領域
	char buffer[1024];  
	//返信データ
	char ans[] = "[server]送信成功";
	char ret;
	int result;	

	while (running)
	{
		//クライアントから通信があるまで待機
		s1 = accept(s, NULL, NULL);
		if (s1 < 0){
			std::cout << "[server]待機エラー" << std::endl;
		}
		memset(&buffer, '\0', sizeof(buffer));
		//クライアントから送信されたデータの受信
		result = recv(s1, buffer, 1024, 0);
		if (result < 0){
			std::cout << "[server]通信エラー" << std::endl;
		}
		printf("[server]%sを受信しました\n", buffer);
		//クライアントへデータを送信する
		result = send(s1, ans, 10, 0);
		printf("[server]クライアントとの接続を終了しました\n");
		closesocket(s1);
		handler->Handle(std::string(buffer));
	}
}