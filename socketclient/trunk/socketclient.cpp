#include <stdio.h>

#include <winsock2.h>



int main(void){

//  SOCKET s;    //�\�P�b�g

  //�ڑ�����T�[�o�̏��
/*
  struct sockaddr_in dest;

  //�ڑ�����T�[�o��IP�A�h���X

  //xxx.xxx.xxx.xxx�̌`���Ŏw�肷��

  char destination[] = "127.0.0.1";



  char buffer[1024];



  //�\�P�b�g�ʐM�̏���

  WSADATA data;

  WSAStartup(MAKEWORD(2,0), &data);



  //�ڑ���i�T�[�o�j�̃A�h���X����ݒ�

  memset(&dest, 0, sizeof(dest));

  //�|�[�g�ԍ��̓T�[�o�v���O�����Ƌ���

  dest.sin_port = htons(7000);//blender���ƍ��킹�� 5/5 kudo   hton�Ńo�C�g�I�[�_�ɕϊ��B

  dest.sin_family = AF_INET;

  dest.sin_addr.s_addr = inet_addr(destination);



  //�\�P�b�g�̐���

  s = socket(AF_INET, SOCK_STREAM, 0);
*/

	//�\�P�b�g�֐��̏����� 5/8 23:00 �����������ƈꔭ�œ������B
    WORD wVersionRequested = MAKEWORD(2,2);
    WSADATA wsaData[1];
    if(WSAStartup(wVersionRequested,wsaData)){
        printf("�\�P�b�g���C�u�����̏������Ɏ��s���܂����B");
        exit(1);
    }


/// C,C++�ł̎��� 5/8 kudo added
SOCKET s;
//struct addrinfo me;

/*
ZeroMemory(&me, sizeof(me)); // �\���̂��[���N���A
me.ai_family = AF_INET;      // IPv4�v���g�R�����w��
me.ai_socktype = SOCK_STREAM;

// �\�P�b�g���쐬
s = socket(hints.ai_family, hints.ai_socktype, 0);
*/
s = socket(AF_INET, SOCK_STREAM, 0);

/// C,C++�ł̎���
// IP�A�h���X���w�肵�ăT�[�o�[�ɐڑ�����
struct sockaddr_in sin; //SocketAddress�\����
char *ip_addr = "localhost";
unsigned short port = 50008;
char buffer[1024];

sin.sin_family = AF_INET;//me.ai_family;
sin.sin_port = htons(port);             // �l�b�g���[�N�o�C�g�I�[�_�[�ɕϊ�
//inet_aton(ip_address, &(sin.sin_addr)); // �l�b�g���[�N�o�C�g�I�[�_�[�ɕϊ�

  //�T�[�o�ւ̐ڑ�

puts("hello!");//for debug
//  if(connect(s, (struct sockaddr *) &dest, sizeof(dest))){ //connect()�́A����������0��Ԃ��B��̓I�Ȏd�l�́H
  if(s==INVALID_SOCKET){  //�\�P�b�g�����Ɏ��s�������̖߂�l
    puts("�\�P�b�g�𐶐��ł��܂���");
    //printf("%s�ɐڑ��ł��܂���ł���\n", ip_addr);

    return -1;

  }

puts("hello!");//for debug
  printf("%s�ɐڑ����܂���\n", ip_addr);


  printf("�T�[�o�ɑ��M���镶�������͂��ĉ������F");

  scanf("%s", buffer);


  //�T�[�o�Ƀf�[�^�𑗐M int send(SOCKET s, const char* buf,int len,int flags);

  send(s, buffer, sizeof(buffer), 0);



  //�T�[�o����f�[�^����M

  char buffer2[128];
  recv(s, buffer2, 128, 0); //������̑O�����S��"�������"�Ŗ��ߐs������镶������

  printf("�� %s\n\n", buffer2);



  // Windows �ł̃\�P�b�g�̏I��

  closesocket(s);

  WSACleanup();



  return 0;

}


//5/7�̃o�b�N�A�b�v�B�isvn������ΕK�v�Ȃ��C�����邪�E�E�E
/* 
  //�T�[�o�ւ̐ڑ�

  if(connect(s, (struct sockaddr *) &dest, sizeof(dest))){

    printf("%s�ɐڑ��ł��܂���ł���\n", destination);

    return -1;

  }

  printf("%s�ɐڑ����܂���\n", destination);


  printf("rotate 1\n");

  scanf("%s", buffer);



  //�T�[�o�Ƀf�[�^�𑗐M int send(SOCKET s, const char* buf,int len,int flags);

  send(s, buffer, sizeof(buffer), 0);



  //�T�[�o����f�[�^����M

  recv(s, buffer, 1024, 0);

  printf("�� %s\n\n", buffer);



  // Windows �ł̃\�P�b�g�̏I��

  closesocket(s);

  WSACleanup();



  return 0;

}
*/
