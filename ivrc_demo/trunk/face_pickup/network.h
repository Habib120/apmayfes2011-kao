#pragma once 
#include <C:\Program Files (x86)\boost\boost_1_46_1\boost\thread/thread.hpp>


#define CLIENT_PORT 50008
#define SERVER_PORT 50011

class SocketClient
{
public:
	void Send(std::string message);
};



