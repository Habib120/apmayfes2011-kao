#include "detectors.h"
#include "structure.h"
#include "network.h"
#include "tracker.h"
#include "extractors.h"
#include <boost/thread/thread.hpp>

class SmileDetectionLoop
{
public :
	SmileDetectionLoop(HeadTracker *t) : tracker(t), stop(false)
	{
		detector = new SmileDetector();
	}
	~SmileDetectionLoop()
	{
		delete detector;
	}
	void Start();
	void Stop();
	void operator()();
protected :
	SmileDetectionLoop() {}
	HeadTracker *tracker;
	SmileDetector *detector;
	boost::thread *loop_thread;
	bool stop;
};