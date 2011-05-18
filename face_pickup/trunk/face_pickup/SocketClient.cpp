#include <stdio.h>
#include <winsock2.h>
#include "network.h"

void SocketClient::Send(std::string message)
{
	 WSADATA wsaData;
	 struct sockaddr_in server;
	 SOCKET sock;
	 char buf[32];
	 char *deststr;
	 unsigned int **addrptr;

	 deststr = "127.0.0.1";

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
	 server.sin_port = htons(PORT);

	 server.sin_addr.S_un.S_addr = inet_addr(deststr);
	 if (server.sin_addr.S_un.S_addr == 0xffffffff) {
		 struct hostent *host;

		 host = gethostbyname(deststr);
		 if (host == NULL) {
			 if (WSAGetLastError() == WSAHOST_NOT_FOUND) {
				 printf("host not found : %s\n", deststr);
			 }
			 return;
		 }

		 addrptr = (unsigned int **)host->h_addr_list;

		 while (*addrptr != NULL) {
			 server.sin_addr.S_un.S_addr = *(*addrptr);

			 // connect()������������loop�𔲂��܂�
			 if (connect(sock,
					(struct sockaddr *)&server,
					sizeof(server)) == 0) {
				break;
			 }

			 addrptr++;
			 // connect�����s�����玟�̃A�h���X�Ŏ����܂�
		 }

		 // connect���S�Ď��s�����ꍇ
		 if (*addrptr == NULL) {
			 printf("connect : %d\n", WSAGetLastError());
			 return;
		 }
	 } else {
		 // inet_addr()�����������Ƃ�

		 // connect�����s������G���[��\�����ďI��
		 if (connect(sock,
						 (struct sockaddr *)&server,
						 sizeof(server)) != 0) {
			 printf("connect : %d\n", WSAGetLastError());
			 return;
		 }
	 }

	 send(sock, "rotate 1", 8, 0);
	 memset(buf, 0, sizeof(buf));
	 /*int n = recv(sock, buf, sizeof(buf), 0);
	 if (n < 0) {
		 printf("recv : %d\n", WSAGetLastError());
		 return;
	 }*/

	 //printf("%d, %s\n", n, buf);

	 closesocket(sock);

	 WSACleanup();

	 return;
}