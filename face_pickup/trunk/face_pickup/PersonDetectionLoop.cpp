#include "network.h"
#include "tracker.h"
#include "detection_thread.h"
#include "extractors.h"
#include <boost/ref.hpp>
#include <boost/format.hpp>
#include <boost/lexical_cast.hpp>
#include <algorithm>

void PersonDetectionLoop::Start()
{
	loop_thread = new boost::thread(boost::ref(*this));
}

void PersonDetectionLoop::Stop()
{
	stop = true;
	loop_thread->join();
}

void PersonDetectionLoop::ResetState()
{
	person = false;
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
			std::string msg = (boost::format("head_pose||%f %f %f %f %f %f") % pose.x % pose.y % pose.z % pose.rx % pose.ry %pose.rz).str();
			client.Send(msg);
		}

		Sleep(50);
	}
}