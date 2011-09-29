#include "network.h"
#include "tracker.h"
#include "detection_thread.h"
#include <boost/ref.hpp>
#include <boost/format.hpp>
#include <boost/lexical_cast.hpp>
#include <algorithm>


extern HANDLE mutex;
//HANDLE _hThread[2];
extern double cam_coord[3];
extern double coord[3];
extern int cam_select;

//座標を送信する
void InfraredPersonDetectionLoop::operator()()
{
	SocketClient client;
	while(1){
		WaitForSingleObject(mutex, INFINITE);
		coord[0] = cam_coord[0];
		coord[1] = cam_coord[1]/cos(M_PI/6);
		coord[2] = cam_coord[2];
		std::string msg = (boost::format("head_pose||%f %f %f %f %f %f") % coord[0] % coord[1] % coord[2] % 0 % 0 % 0).str();
		cout << msg << endl;
		//client.Send(msg);
		ReleaseMutex(mutex);
	}
}

InfraredPersonDetectionLoop::InfraredPersonDetectionLoop()
{
}

//サーバーをスタートする．
void InfraredPersonDetectionLoop::Start()
{
	HANDLE _thread;
	DWORD id;
	srand(GetTickCount());
	// Query for number of connected cameras
	int numCams = CLEyeGetCameraCount();
	if(numCams == 0)
	{
		printf("No PS3Eye cameras detected\n");
	}
	printf("Found %d cameras\n", numCams);
	mutex = CreateMutex(NULL, FALSE, NULL);
	
	cam = new CLEyeCameraCapture*[numCams];

	for(int i = 0; i < numCams; i++)
	{
		char windowName[64];
		// Query unique camera uuid
		GUID guid = CLEyeGetCameraUUID(i);
		printf("Camera %d GUID: [%08x-%04x-%04x-%02x%02x-%02x%02x%02x%02x%02x%02x]\n", 
						i+1, guid.Data1, guid.Data2, guid.Data3,
						guid.Data4[0], guid.Data4[1], guid.Data4[2],
						guid.Data4[3], guid.Data4[4], guid.Data4[5],
						guid.Data4[6], guid.Data4[7]);
		sprintf(windowName, "Camera Window %d", i+1);
		cam[i] = new CLEyeCameraCapture(windowName, guid, CLEYE_MONO_PROCESSED, CLEYE_VGA, 60);
		printf("Starting capture on camera %d\n", i+1);
		cam[i]->cam_num = i;
		cam[i]->StartCapture();
	}
	loop_thread = new boost::thread(boost::ref(*this));
}

void InfraredPersonDetectionLoop::Stop()
{
    CloseHandle(mutex);
	for(int i = 0; i < numCams; i++)
	{
		printf("Stopping capture on camera %d\n", i+1);
		cam[i]->StopCapture();
		delete cam[i];
	}
}