#include "network.h"
#include "tracker.h"
#include "detection_thread.h"
#include "extractors.h"
#include <boost/ref.hpp>
using namespace std;

void FaceComDetectionLoop::Start()
{
	loop_thread = new boost::thread(boost::ref(*this));
}

void FaceComDetectionLoop::Stop()
{
	stop = true;
	loop_thread->join();
}

void FaceComDetectionLoop::operator()()
{
	SocketClient client;
	while (!stop)
	{
		HeadData *data = tracker->GetCurrentHeadData();
		std::cout << data << std::endl;
		if (data != 0)
		{
			FaceComDetectionResult result = detector->Detect(*data);
			if (result.HasData())
			{
				if (result.is_smiling && !is_smiling)
				{
					client.Send("customer_smiled");
				}
				else if (!result.is_smiling && is_smiling)
				{
					client.Send("customer_dissmiled");
				}
				is_smiling = result.is_smiling;
			}
			data->ReleaseImage();
			delete data;
		}
		else 
		{
			Sleep(50);
		}
	}
}

