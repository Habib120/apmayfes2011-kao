#include "detection_thread.h"
#include <boost/format.hpp>
#include <boost/cast.hpp>
#include <boost/tokenizer.hpp>
#include <boost/foreach.hpp>
#include <string>

#define PAGE_THRESHOLD 506

BookObserver::BookObserver()
{
	//serial = new SerialAdapter();
}

void BookObserver::Start()
{
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
	/**
	boost::char_separator<char> sep(" ");
	boost::tokenizer<boost::char_separator<char>> tokenizer(rcv, sep);
	std::vector<int> sensor_values;

	BOOST_FOREACH(string t, tokenizer)
	{
		try {
			int v = boost::numeric_cast<int>(t);
			sensor_values.push_back(v);
		}
		catch (boost::bad_numeric_cast e){
			std::cerr << "Invalid sensor value : " << t << endl;
		}
	}
	//8ŒÂ’l‚ª•Ô‚Á‚Ä‚­‚ê‚Î³í
	if (sensor_values.size() != 8)
	{
		std::cerr << "Failed to parse COM response : " << rcv << endl;
		return;
	}

	std::vector<int> open_pages = getOpenPages(sensor_values);
	std::cout << "open_pages : ";
	BOOST_FOREACH(int& i, open_pages)
	{
		std::cout << i << ", ";
	}
	std::cout << endl;
	*/
}

void BookObserver::operator()()
{
	/*
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
	*/
}