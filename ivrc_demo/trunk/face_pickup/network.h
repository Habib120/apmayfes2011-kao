#pragma once 
#include <boost\thread/thread.hpp>


#define CLIENT_PORT 50008
#define SERVER_PORT 50011

class SocketClient
{
public:
	void Send(std::string message);
};



