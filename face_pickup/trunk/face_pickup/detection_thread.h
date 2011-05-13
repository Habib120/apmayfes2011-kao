#include "detectors.h"
#include "structure.h"
#include "network.h"

class SmileDetectionLoop
{
public :
	SmileDetectionLoop(FaceTracker *tracker);
	void Start();
	void Stop();
	void operator()();
protected :
	FaceTracker *tracker;
}