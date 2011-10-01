#include "detection_thread.h"
#include <boost/format.hpp>
#include <boost/lexical_cast.hpp>
#include <boost/cast.hpp>
#include <boost/tokenizer.hpp>
#include <boost/foreach.hpp>
#include <boost/algorithm/string.hpp>
#include <string>

#define PAGE_THRESHOLD 506

BookObserver::BookObserver()
{
	serial = new SerialAdapter();
	if (!serial->Init())
	{
		std::cerr << "Failed to serial port" << std::endl;
	}
}

void BookObserver::Start()
{
	stop = false;
	loop_thread = new boost::thread(boost::ref(*this));
}

void BookObserver::Stop()
{
	stop = true;
	loop_thread->join();
}

std::vector<int> BookObserver::getOpenPages(std::vector<int> sensor_values)
{
	std::vector<int> open_pages;
	for (int i = 0; i < sensor_values.size(); i++)
	{
		if (sensor_values.at(i) > PAGE_THRESHOLD)
		{
			open_pages.push_back(i);
		}
	}
	return open_pages;
}

void BookObserver::processReceivedText(std::string rcv, SocketClient *client)
{
	boost::char_separator<char> sep(" ");
	boost::tokenizer<boost::char_separator<char>> tokenizer(rcv, sep);
	std::vector<int> sensor_values;

	BOOST_FOREACH(string t, tokenizer)
	{
		//空白，改行は無視する．
		if (t == "" || t == "\r\n")
			continue;
		try {
			int v = boost::lexical_cast<int>(t);
			sensor_values.push_back(v);
		} catch (boost::bad_lexical_cast e) {
			std::cerr << "Invalid sensor value '" << t  << "'" << endl;
		}
	}
	//8個値が返ってくれば正常
	if (sensor_values.size() != 8)
	{
		std::cerr << "Failed to parse COM response : " << rcv << endl;
		return;
	}

	std::vector<int> open_pages = getOpenPages(sensor_values);
	string msg = "book_page||";
	BOOST_FOREACH(int& i, open_pages)
	{
		try {
			msg = msg + boost::lexical_cast<std::string>(i+1) + " ";
		} catch (boost::bad_lexical_cast e) {
			std::cerr << "Invalid sensor value '" << i  << "'" << endl;
		}
	}
	boost::trim(msg);
	std::cout << msg << endl;
	client->Send(msg);
}

void BookObserver::operator()()
{
	string rcv;
	serial->Start();
	SocketClient client;
	while (!stop)
	{
		if (serial->GetReadQueueCount() > 0) {
			serial->Read(&rcv);
			processReceivedText(rcv, &client);
		}
	}
}