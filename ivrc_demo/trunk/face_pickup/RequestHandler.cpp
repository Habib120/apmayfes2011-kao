#include "network.h"
#include "connection.h"
#include "cv.h"
#include <boost/format.hpp>
#include <ctime>
#include <windows.h>

#define MOULIN_PHOTO_SAVE_DIR "C:\\photo\\"
#define MOULIN_PHOTO_LATEST_DIR "C:\\photo\\latest\\"

int latestcount = 0;

void RequestHandler::Handle(std::string msg)
{
	std::vector<std::string> results;
	std::string::size_type index = msg.find("||");
	std::string header, body;
	if (index == std::string::npos)
	{
		header = msg;
	}
	else
	{
		header = msg.substr(0, index);
		body = msg.substr(index + 2);
	}
	std::cout << "header : " << header << " body : " << body << std::endl;

	if (header == "reset_game")
	{

	}
}