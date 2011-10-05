#include <stdio.h>
#include <winsock2.h>
#include "network.h"

SocketClient::SocketClient(std::string host) : _host(host)
{
}

void SocketClient::Send(std::string message)
{
#ifndef _DEBUG
	std::cout << message << std::endl;
	return;
#endif
	 WSADATA wsaData;
	 struct sockaddr_in server;
	 SOCKET sock;
	 char buf[1024];
	 char *deststr;
	 unsigned int **addrptr;


	 if (WSAStartup(MAKEWORD(2,0), &wsaData) != 0) {
		 printf("WSAStartup failed\n");
		 return;
	 }

	 sock = socket(AF_INET, SOCK_STREAM, 0);
	 if (sock == INVALID_SOCKET) {
		 printf("socket : %d\n", WSAGetLastError());
		 return;
	 }

	 server.sin_family = AF_INET;
	 server.sin_port = htons(CLIENT_PORT);

	 server.sin_addr.S_un.S_addr = inet_addr(_host.c_str());
	 if (server.sin_addr.S_un.S_addr == 0xffffffff) {
		 struct hostent *host;

		 host = gethostbyname(_host.c_str());
		 if (host == NULL) {
			 if (WSAGetLastError() == WSAHOST_NOT_FOUND) {
				 printf("host not found : %s\n", deststr);
			 }
			 return;
		 }

		 addrptr = (unsigned int **)host->h_addr_list;

		 while (*addrptr != NULL) {
			 server.sin_addr.S_un.S_addr = *(*addrptr);

			 // connect()が成功したらloopを抜けます
			 if (connect(sock,
					(struct sockaddr *)&server,
					sizeof(server)) == 0) {
				break;
			 }

			 addrptr++;
			 // connectが失敗したら次のアドレスで試します
		 }

		 // connectが全て失敗した場合
		 if (*addrptr == NULL) {
			 printf("connect : %d\n", WSAGetLastError());
			 return;
		 }
	 } else {
		 // inet_addr()が成功したとき

		 // connectが失敗したらエラーを表示して終了
		 if (connect(sock,
						 (struct sockaddr *)&server,
						 sizeof(server)) != 0) {
			 printf("connect : %d\n", WSAGetLastError());
			 return;
		 }
	 }

	 send(sock, message.c_str(), message.size(), 0);
	 memset(buf, 0, sizeof(buf));
	 int n = recv(sock, buf, sizeof(buf), 0);
	 if (n < 0) {
		 printf("recv : %d\n", WSAGetLastError());
		 return;
	 }

	 printf("%d, %s\n", n, buf);

	 closesocket(sock);

	 WSACleanup();

	 return;
}
