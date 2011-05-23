#include "network.h"
#include "tracker.h"
#include "detection_thread.h"
#include "extractors.h"
#include <boost/ref.hpp>
#include <boost/format.hpp>

void PersonDetectionLoop::Start()
{
	loop_thread = new boost::thread(boost::ref(*this));
}

void PersonDetectionLoop::Stop()
{
	stop = true;
	loop_thread->join();
}

void PersonDetectionLoop::operator()()
{
	SocketClient client;
	while (!stop)
	{
		HeadPose pose = tracker->GetCurrentHeadPose();
		double p;
		if (pose.isValueSet())
		{
			p = 3;
			if (person)
			{
				//client.Send("Hello, I'm cpp client");
				std::string msg = (boost::format("head_pose||%f %f %f %f %f %f") % pose.x % pose.y % pose.z % pose.rx % pose.ry %pose.rz).str();
				client.Send(msg);
			}
		}
		else
		{
			p = -3;
		}
		person_confidence = (person_confidence * (RUN_AVG_NUM - 1) + p) / RUN_AVG_NUM;

		if (!person && person_confidence > 0.5)
		{
			person = true;
			person_confidence = 3;
			client.Send("customer_comein");
		}
		else if(person && person_confidence < 0.5)
		{
			person = false;
			person_confidence = 0;
			client.Send("customer_leave");
		}
		Sleep(50);
	}
}