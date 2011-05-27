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

void PersonDetectionLoop::operator()()
{
	SocketClient client;
	while (!stop)
	{
		HeadPose pose = tracker->GetCurrentHeadPose();
		double p;
		if (pose.isValueSet())
		{
			if (!person)
			{
				while (true)
				{
					FaceComDetector detector;
					IplImage* cam_image = tracker->GetCurrentCamImage();
					IplImage* simage = cvCreateImage(cvSize(320, 240), IPL_DEPTH_8U, 3);
					cvResize(cam_image, simage);
					std::vector<FaceComDetectionResult> results = detector.Detect(simage);
					if (results.size() > 0)
					{
						int pcount = results.size();
						std::string msg = "customer_comein||";
						msg += boost::lexical_cast<std::string>(pcount);
						for (int i = 0; i < pcount; i++)
						{

							std::cout << "is_male : " << results.at(i).is_male << " confidence : " << results.at(i).con_gender << std::endl;
							msg += (boost::format(" {%s,%s}") % std::string(results.at(i).is_male ? "male" : "female") % std::string(results.at(i).has_glasses ? "glasses" : "no_glasses")).str();
						}
						client.Send(msg);
						cvReleaseImage(&cam_image);
						break;
					}
					cvReleaseImage(&cam_image);
				}
				person = true;
			}
			if (pose.ry < -3.14/8.0 )
			{	
				client.Send("customer_turnright");
			}
			else if (pose.ry > 3.14/8.0)
			{
				client.Send("customer_turnleft");
			}
			else if (pose.rx > 3.14/8.0)
			{
				client.Send("customer_turnup");
			}
			else if (pose.rx < -3.14/16.0)
			{
				client.Send("customer_turndown");
			}
			else if (std::abs(pose.ry) < 3.14/16.0 && (std::abs(pose.rz) < 3.14/16.0 && std::abs(pose.rx) < 3.14/16.0))
			{
				client.Send("customer_turnfront");
			}
			
			std::string msg = (boost::format("head_pose||%f %f %f %f %f %f") % pose.x % pose.y % pose.z % pose.rx % pose.ry %pose.rz).str();
			client.Send(msg);
		}

		Sleep(50);
	}
}