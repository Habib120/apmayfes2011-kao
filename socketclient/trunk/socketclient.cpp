#include <stdio.h>

#include <winsock2.h>



int main(void){

  SOCKET s;    //�\�P�b�g

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

  dest.sin_port = htons(7000);//blender���ƍ��킹�� 5/5 kudo

  dest.sin_family = AF_INET;

  dest.sin_addr.s_addr = inet_addr(destination);



  //�\�P�b�g�̐���

  s = socket(AF_INET, SOCK_STREAM, 0);
*/

/// C,C++�ł̎���
// IP�A�h���X���w�肵�ăT�[�o�[�ɐڑ�����
struct sockaddr_in sin;
char *ip_address = "192.168.0.1"
unsigned short port = 7000;

sin.sin_family = me.ai_family;
sin.sin_port = htons(port);             // �l�b�g���[�N�o�C�g�I�[�_�[�ɕϊ�
inet_aton(ip_address, &(sin.sin_addr)); // �l�b�g���[�N�o�C�g�I�[�_�[�ɕϊ�


  //�T�[�o�ւ̐ڑ�

  if(connect(s, (struct sockaddr *) &dest, sizeof(dest))){

    printf("%s�ɐڑ��ł��܂���ł���\n", destination);

    return -1;

  }

  printf("%s�ɐڑ����܂���\n", destination);



  printf("rotate 1\n");

  scanf("%s", buffer);



  //�T�[�o�Ƀf�[�^�𑗐M

  send(s, buffer, sizeof(buffer), 0);



  //�T�[�o����f�[�^����M

  recv(s, buffer, 1024, 0);

  printf("�� %s\n\n", buffer);



  // Windows �ł̃\�P�b�g�̏I��

  closesocket(s);

  WSACleanup();



  return 0;

}