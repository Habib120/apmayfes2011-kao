#include <winsock2.h>
#include "connection.h"
#include <iostream>
#include <stdio.h>


void SocketServer::Start()
{
	int result = 0;
	//�ڑ����񏉊���
	memset(&source, 0, sizeof(source));
	//�A�h���X�t�@�~���[��INET(����)
	source.sin_family = AF_INET;
	//�|�[�g�ԍ��̓N���C�A���g�v���O�����Ƌ���
	source.sin_port = htons(SERVER_PORT);
	//�ǂ�����̐ڑ��v�����󂯓����
	source.sin_addr.s_addr = htonl(INADDR_ANY);

	//�\�P�b�g�ʐM�̊J�n����
	WSADATA data;	
	result = WSAStartup(MAKEWORD(2, 0), &data);
	if (result < 0){
		printf("%d\n", GetLastError());
		printf("[server]�\�P�b�g�ʐM�����G���[\n");
	}

	//�\�P�b�g�̐���
	s = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (s < 0){
		printf("%d\n", GetLastError());
		printf("[server]�\�P�b�g�����G���[\n");
	}

	//�\�P�b�g�̃o�C���h
	result = bind(s, (struct sockaddr *)&source, sizeof(source));
	if (result < 0){
		printf("%d\n", GetLastError());
		printf("[server]�o�C���h�G���[\n");
	}

	//�ڑ��̋���
	result = listen(s, 1);
	if (result < 0){
		printf("[server]�ڑ����G���[\n");
	}
	printf("[server]�ڑ��J�n\n");
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
	//�\�P�b�g�ʐM�̏I��
	WSACleanup();
}

void SocketServer::operator()()
{
	//��M�f�[�^�̃o�b�t�@�̈�
	char buffer[1024];  
	//�ԐM�f�[�^
	char ans[] = "[server]���M����";
	char ret;
	int result;	

	while (running)
	{
		//�N���C�A���g����ʐM������܂őҋ@
		s1 = accept(s, NULL, NULL);
		if (s1 < 0){
			std::cout << "[server]�ҋ@�G���[" << std::endl;
		}
		memset(&buffer, '\0', sizeof(buffer));
		//�N���C�A���g���瑗�M���ꂽ�f�[�^�̎�M
		result = recv(s1, buffer, 1024, 0);
		if (result < 0){
			std::cout << "[server]�ʐM�G���[" << std::endl;
		}
		printf("[server]%s����M���܂���\n", buffer);
		//�N���C�A���g�փf�[�^�𑗐M����
		result = send(s1, ans, 10, 0);
		printf("[server]�N���C�A���g�Ƃ̐ڑ����I�����܂���\n");
		closesocket(s1);
		handler->Handle(std::string(buffer));
	}
}