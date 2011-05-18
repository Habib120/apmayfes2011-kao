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
	while (!stop)
	{
		HeadData *data = tracker->GetCurrentHeadData();
		std::cout << data << std::endl;
		if (data != 0)
		{
			FaceComDetectionResult result = detector->Detect(*data);
			std::string msg = result.HasData() ? "detection success!" : "detection failure!";
			cout << msg << endl;
			cout << "is_smiling : " << result.is_smiling << " confidence : " << result.con_smiling << endl;
			cout << "is_male : "    << result.is_male    << " confidence : " << result.con_gender  << endl;
			cout << "has_glasses : "<< result.has_glasses<< " confidence : " << result.con_glasses << endl;
			data->ReleaseImage();
			delete data;
		}
		else 
		{
			Sleep(50);
		}
	}
}

