#include "tracker.h"
#include "detection_thread.h"
#include "extractors.h"
#include <boost/ref.hpp>

void SmileDetectionLoop::Start()
{
	loop_thread = new boost::thread(boost::ref(*this));
}

void SmileDetectionLoop::Stop()
{
	stop = true;
	loop_thread->join();
}

void SmileDetectionLoop::operator()()
{
	while (!stop)
	{
		HeadData *data = tracker->GetCurrentHeadData();
		std::cout << data << std::endl;
		if (data != 0)
		{
			std::string result = detector->Detect(*data);
			std::cout << result << std::endl;

			data->ReleaseImage();
			delete data;
		}
		else 
		{
			Sleep(50);
		}
	}
}