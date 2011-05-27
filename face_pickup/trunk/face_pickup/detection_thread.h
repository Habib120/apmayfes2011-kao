//network.hを一番初めにインクルードする必要がある
#ifndef __DETECTION_THREAD
#define __DETECTION_THREAD
#endif
#include "network.h"
#include "detectors.h"
#include "structure.h"
#include "tracker.h"
#include "extractors.h"
#include <boost/thread/thread.hpp>

#pragma once

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

class FaceComDetectionLoop
{
public :
	FaceComDetectionLoop(HeadTracker *t) : tracker(t), stop(false), is_smiling(false)
	{
		detector = new FaceComDetector();
	}
	~FaceComDetectionLoop()
	{
		delete detector;
	}
	void Start();
	void Stop();
	void operator()();
protected :
	FaceComDetectionLoop() {}
	HeadTracker *tracker;
	FaceComDetector *detector;
	boost::thread *loop_thread;
	bool is_smiling;
	bool stop;
};

class PersonDetectionLoop
{
public :
	PersonDetectionLoop(HeadTracker *t) : tracker(t), stop(false), person(false), person_confidence(0)
	{
	}
	void Start();
	void Stop();
	void operator()();
	void ResetState();
protected :
	PersonDetectionLoop() {}
	HeadTracker *tracker;
	boost::thread *loop_thread;
	bool stop;
	bool person;
	double person_confidence;
	static const int RUN_AVG_NUM = 70;
};