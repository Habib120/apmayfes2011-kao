#include "structure.h"
#include "cv.h"
#include "highgui.h"
#include <boost/thread/mutex.hpp>
#include <boost/thread/thread.hpp>

#pragma once

class HeadTracker
{
public :
	HeadTracker()
		: cap(0), data(0), stop(false){}
	void Start(bool with_dbg = true);
	void Stop();
	HeadData* GetCurrentHeadData();
	IplImage* GetCurrentCamImage();
	void operator()();
private :
	boost::thread *loop_thread;
	HeadData *data;
	IplImage *cframe;
	bool stop;
	bool with_dbg;
	smEngineHandle engine;
	smVideoDisplayHandle video_display_handle;
	CvCapture *cap;
};
