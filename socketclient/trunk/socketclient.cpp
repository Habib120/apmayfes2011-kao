#include <stdio.h>

#include <winsock2.h>


int main(void){

  SOCKET s;    //�\�P�b�g

  //�ڑ�����T�[�o�̏��

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

  dest.sin_port = htons(50008);

  dest.sin_family = AF_INET;

  dest.sin_addr.s_addr = inet_addr(destination);



  //�\�P�b�g�̐���

  s = socket(AF_INET, SOCK_STREAM, 0);
  puts("hello!");



  //�T�[�o�ւ̐ڑ� //�����Ŏ~�܂�D5.13 -> �P�Ɏ��s���x������������

  //connect(Socket, (struct sockaddr *)&sin, sizeof(sin));


//int tmp = connect(s, (struct sockaddr *)&dest, sizeof(dest)); //5.13 kudo added for debug

//printf("%d\n",tmp); //for debug

  if(connect(s, (struct sockaddr *) &dest, sizeof(dest))){ //connect()�͎��s�����-1��Ԃ�
	  

    printf("%s�ɐڑ��ł��܂���ł���\n", destination);

    return -1;

  }
//connect(s, (struct sockaddr *) &dest, sizeof(dest)); //5.13 kudo added

//puts("hello!2"); //for debug
  printf("%s�ɐڑ����܂���\n", destination);


//puts("hello!2"); //for debug
  printf("�T�[�o�ɑ��M���镶�������͂��ĉ�����\n");

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