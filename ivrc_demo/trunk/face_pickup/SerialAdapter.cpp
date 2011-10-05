
#include "util.h"

#define COM_NAME "COM6"
#define MAX_QUE 4096
#define DEF_WAIT (1000/60)

SerialAdapter::SerialAdapter()
:hPort(NULL)
,hThread(NULL)
,hMutex(NULL)
,f_loop(true)
,f_start(false)
,wait(DEF_WAIT)
{
}

SerialAdapter::~SerialAdapter(){
}

bool SerialAdapter::Init()
{
	//�ʐM���s�����߂̃X���b�h�Ɠ������߂̃~���[�e�N�X���쐬
	unsigned int id;
	hMutex=(HANDLE)CreateMutex(NULL,false,NULL);
	if(hMutex==NULL){
		return false;
	}
	hThread=(HANDLE)_beginthreadex(NULL,0,SerialAdapter::Launch,this,0,&id);
	if(hThread==NULL){
		CloseHandle(hMutex);
		return false;
	}

	return true;
}
unsigned int SerialAdapter::Launch(void* arg){
	((SerialAdapter*)arg)->Loop();
	return 0;
}
void SerialAdapter::Start(){
	f_start=true;
}
void SerialAdapter::Stop(){
	f_start=false;
}
bool SerialAdapter::IsStart(){
	return f_start;
}
bool SerialAdapter::IsActive(){
	DWORD dw;
	GetExitCodeThread(hThread,&dw);
	return f_loop&&(dw==STILL_ACTIVE);
}
void SerialAdapter::SetWait(int w){
	wait=w;
}

void SerialAdapter::Write(string str){
	//���b�N�����㑗�M�L���[�ɕ������ς�
	WaitForSingleObject(hMutex,INFINITE);
	outque.push(str);
	ReleaseMutex(hMutex);
}
void SerialAdapter::Read(string* str){
	//���b�N�������M�L���[����擪�̕���������o��
	WaitForSingleObject(hMutex,INFINITE);
	//if(!inque.empty()){
	//	(*str)=inque.top();
	//	inque.pop();
	//}
	if (input != "")
	{
		(*str) = input;
	}
	ReleaseMutex(hMutex);
}
int SerialAdapter::GetReadQueueCount(){
	int num;
	WaitForSingleObject(hMutex,INFINITE);
	//num=inque.size();
	num = input != "" ? 1 : 0;
	ReleaseMutex(hMutex);
	return num;
}
int SerialAdapter::GetWriteQueueCount(){
	int num;
	WaitForSingleObject(hMutex,INFINITE);
	num=outque.size();
	ReleaseMutex(hMutex);
	return num;
}

void SerialAdapter::Loop(){
	//�V���A���ʐM�̏�����
	strcpy_s(comName,COM_NAME);
	ncomcfg=sizeof(comcfg);
	hPort = CreateFileA(
		comName,
		GENERIC_READ|GENERIC_WRITE,
		0,
		NULL,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		NULL
		);
	if(hPort == INVALID_HANDLE_VALUE){
		puts("CreateFile error");
		return;
	}
	if(!SetupComm(hPort,MAX_QUE,MAX_QUE)){
		puts("SetupComm error");
		return;
	}
	if(!PurgeComm(hPort,PURGE_TXABORT | PURGE_RXABORT | PURGE_TXCLEAR | PURGE_RXCLEAR)){
		puts("PurgeComm error");
		return;
	}

	//�R���t�B�O�A�^�C���A�E�g�̐ݒ�
	DCB dcb;
	GetCommState(hPort,&dcb);
	SetupCommState(&dcb);
	if(!SetCommState(hPort,&dcb)){
		puts("SetCommState error");
		return;
	}

	GetCommConfig(hPort,&comcfg,&ncomcfg);
	SetupCommConfig(&comcfg);
	if(!SetCommConfig(hPort, &comcfg,ncomcfg)){
		puts("SetCommConfig error");
		return;
	}	

	COMMTIMEOUTS timeout;
	SetupTimeout(&timeout);
	if(!SetCommTimeouts(hPort, &timeout)){
		puts("SetCommTimeouts error");
		return;
	}
	
	DWORD dwError,dwRead,dwWrite;
	COMSTAT ComStat;
	string res;
	string res_last;
	string send;
	char inbuf[MAX_QUE];
	char outbuf[MAX_QUE];

	//���C�����[�v
	while(f_loop){
		//start�̃t���O�������Ă���Ƃ��A�L���[�����b�N���ĒʐM���s��
		if(f_start){
			WaitForSingleObject(hMutex,INFINITE);

			//���M
			//���M�L���[����f�[�^�����ׂĎ��o���ď�������
			while(!outque.empty()){
				send=outque.front();
				outque.pop();
				sprintf_s(outbuf,MAX_QUE,"%s",send.c_str());
				int num = strlen(outbuf);
				if(!WriteFile(hPort,outbuf,num+1,&dwWrite,NULL)){
					puts("writefile error\ndd");
					f_loop=false;
					break;
				}
			}

			//��M
			//��M�L���[�̒��g��S�ĂƂ肾���������ăL���[�ɐς�
			ClearCommError(hPort,&dwError,&ComStat);
			if(ComStat.cbInQue){
				if(ReadFile(hPort,inbuf,ComStat.cbInQue,&dwRead,NULL )){
					inbuf[dwRead]='\0';
					res=res_last+inbuf;
					for(int x=0;x+1<(signed)res.length();x++){
						//���s�ŕ����ăL���[�ɒǉ�
						if(res[x]=='\r' && res[x+1]=='\n'){
							string sub = res.substr(0,x+2);
							input = sub;
							res=res.substr(x+2);
							x=0;
						}
					}
					res_last=res;
				}
				else{
					puts("readfile error\n");
					f_loop=false;
					break;
				}
			}

			ReleaseMutex(hMutex);
		}

		//�L�[����
		char c;
		if(_kbhit()){
			c=_getch();
			if(c=='e'){
				f_loop=false;
				break;
			}
		}
		Sleep(wait);
	}
	CloseHandle(hPort);
	hPort=NULL;
}

void SerialAdapter::Close(void){
	if(f_loop){
		f_loop=false;
		WaitForSingleObject(hThread,INFINITE);
	}
	if(hThread!=NULL){
		CloseHandle(hThread);
	}
	if(hMutex!=NULL){
		CloseHandle(hMutex);
	}
}


void SerialAdapter::SetupTimeout(COMMTIMEOUTS *timeout){
	//�^�C���A�E�g�̐ݒ�
	timeout->ReadIntervalTimeout = 500;
	timeout->ReadTotalTimeoutMultiplier = 0;
	timeout->ReadTotalTimeoutConstant = 500;
	timeout->WriteTotalTimeoutMultiplier = 0;
	timeout->WriteTotalTimeoutConstant = 500;
}
void SerialAdapter::SetupCommConfig(COMMCONFIG* comcfg){
	//�R���t�B�O�̐ݒ�
}

void SerialAdapter::SetupCommState(DCB *dcb){
	dcb->BaudRate=9600;
	dcb->ByteSize=8;
	dcb->Parity=NOPARITY;
	dcb->StopBits=ONESTOPBIT;
}