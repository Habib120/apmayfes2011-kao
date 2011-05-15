#include <winsock2.h>
#include <boost/thread/thread.hpp>
#include <windows.h>

#pragma once 

#define PORT 50008

class SocketClient
{
public:
	void Send(std::string message);
};

class SocketServer
{
public:
	SocketServer()
		:running(false)
	{
	}
	void Start();
	void Stop();
	void operator()();
protected:
	bool running;
	boost::thread *loop_thread;
	SOCKET s;
	SOCKET s1;
	struct sockaddr_in source;

};