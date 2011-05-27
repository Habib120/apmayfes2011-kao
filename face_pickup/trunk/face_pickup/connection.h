#include "tracker.h"
#include "network.h"
#include "detection_thread.h"


class RequestHandler
{
public:
	RequestHandler(HeadTracker *tracker, PersonDetectionLoop *ploop)
		:tracker(tracker), ploop(ploop)
	{
	}
	void Handle(std::string msg);
protected:
	HeadTracker *tracker;
	PersonDetectionLoop *ploop;
};

class SocketServer
{
public:
	SocketServer(HeadTracker *tracker, PersonDetectionLoop *loop)
		:running(false), tracker(tracker), ploop(ploop)
	{
		handler = new RequestHandler(tracker, ploop);
	}
	void Start();
	void Stop();
	void operator()();
protected:
	bool running;
	boost::thread *loop_thread;
	HeadTracker *tracker;
	SOCKET s;
	SOCKET s1;
	struct sockaddr_in source;
	RequestHandler *handler;
	PersonDetectionLoop *ploop;
};