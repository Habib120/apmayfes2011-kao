#include <stdio.h>

#include <winsock2.h>



int main(void){

  SOCKET s;    //ソケット

  //接続するサーバの情報
/*
  struct sockaddr_in dest;

  //接続するサーバのIPアドレス

  //xxx.xxx.xxx.xxxの形式で指定する

  char destination[] = "127.0.0.1";



  char buffer[1024];



  //ソケット通信の準備

  WSADATA data;

  WSAStartup(MAKEWORD(2,0), &data);



  //接続先（サーバ）のアドレス情報を設定

  memset(&dest, 0, sizeof(dest));

  //ポート番号はサーバプログラムと共通

  dest.sin_port = htons(7000);//blender側と合わせた 5/5 kudo

  dest.sin_family = AF_INET;

  dest.sin_addr.s_addr = inet_addr(destination);



  //ソケットの生成

  s = socket(AF_INET, SOCK_STREAM, 0);
*/

/// C,C++での実装
// IPアドレスを指定してサーバーに接続する
struct sockaddr_in sin;
char *ip_address = "192.168.0.1"
unsigned short port = 7000;

sin.sin_family = me.ai_family;
sin.sin_port = htons(port);             // ネットワークバイトオーダーに変換
inet_aton(ip_address, &(sin.sin_addr)); // ネットワークバイトオーダーに変換


  //サーバへの接続

  if(connect(s, (struct sockaddr *) &dest, sizeof(dest))){

    printf("%sに接続できませんでした\n", destination);

    return -1;

  }

  printf("%sに接続しました\n", destination);



  printf("rotate 1\n");

  scanf("%s", buffer);



  //サーバにデータを送信

  send(s, buffer, sizeof(buffer), 0);



  //サーバからデータを受信

  recv(s, buffer, 1024, 0);

  printf("→ %s\n\n", buffer);



  // Windows でのソケットの終了

  closesocket(s);

  WSACleanup();



  return 0;

}