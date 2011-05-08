#include <stdio.h>

#include <winsock2.h>



int main(void){

//  SOCKET s;    //ソケット

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

  dest.sin_port = htons(7000);//blender側と合わせた 5/5 kudo   htonでバイトオーダに変換。

  dest.sin_family = AF_INET;

  dest.sin_addr.s_addr = inet_addr(destination);



  //ソケットの生成

  s = socket(AF_INET, SOCK_STREAM, 0);
*/

	//ソケット関数の初期化 5/8 23:00 これを加えると一発で動いた。
    WORD wVersionRequested = MAKEWORD(2,2);
    WSADATA wsaData[1];
    if(WSAStartup(wVersionRequested,wsaData)){
        printf("ソケットライブラリの初期化に失敗しました。");
        exit(1);
    }


/// C,C++での実装 5/8 kudo added
SOCKET s;
//struct addrinfo me;

/*
ZeroMemory(&me, sizeof(me)); // 構造体をゼロクリア
me.ai_family = AF_INET;      // IPv4プロトコルを指定
me.ai_socktype = SOCK_STREAM;

// ソケットを作成
s = socket(hints.ai_family, hints.ai_socktype, 0);
*/
s = socket(AF_INET, SOCK_STREAM, 0);

/// C,C++での実装
// IPアドレスを指定してサーバーに接続する
struct sockaddr_in sin; //SocketAddress構造体
char *ip_addr = "localhost";
unsigned short port = 50008;
char buffer[1024];

sin.sin_family = AF_INET;//me.ai_family;
sin.sin_port = htons(port);             // ネットワークバイトオーダーに変換
//inet_aton(ip_address, &(sin.sin_addr)); // ネットワークバイトオーダーに変換

  //サーバへの接続

puts("hello!");//for debug
//  if(connect(s, (struct sockaddr *) &dest, sizeof(dest))){ //connect()は、成功したら0を返す。具体的な仕様は？
  if(s==INVALID_SOCKET){  //ソケット生成に失敗した時の戻り値
    puts("ソケットを生成できません");
    //printf("%sに接続できませんでした\n", ip_addr);

    return -1;

  }

puts("hello!");//for debug
  printf("%sに接続しました\n", ip_addr);


  printf("サーバに送信する文字列を入力して下さい：");

  scanf("%s", buffer);


  //サーバにデータを送信 int send(SOCKET s, const char* buf,int len,int flags);

  send(s, buffer, sizeof(buffer), 0);



  //サーバからデータを受信

  char buffer2[128];
  recv(s, buffer2, 128, 0); //文字列の前方が全て"ﾌﾌﾌﾌﾌﾌﾌ"で埋め尽くされる文字化け

  printf("→ %s\n\n", buffer2);



  // Windows でのソケットの終了

  closesocket(s);

  WSACleanup();



  return 0;

}


//5/7のバックアップ。（svnがあれば必要ない気もするが・・・
/* 
  //サーバへの接続

  if(connect(s, (struct sockaddr *) &dest, sizeof(dest))){

    printf("%sに接続できませんでした\n", destination);

    return -1;

  }

  printf("%sに接続しました\n", destination);


  printf("rotate 1\n");

  scanf("%s", buffer);



  //サーバにデータを送信 int send(SOCKET s, const char* buf,int len,int flags);

  send(s, buffer, sizeof(buffer), 0);



  //サーバからデータを受信

  recv(s, buffer, 1024, 0);

  printf("→ %s\n\n", buffer);



  // Windows でのソケットの終了

  closesocket(s);

  WSACleanup();



  return 0;

}
*/
