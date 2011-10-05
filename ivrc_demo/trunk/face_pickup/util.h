
#pragma once

#include <cv.h>
#include "structure.h"

#include <iterator>
#include <vector>
#include <list>
#include <map>
#include <set>
#include <deque>
#include <queue>
#include <stack>
#include <bitset>
#include <algorithm>
#include <functional>
#include <numeric>
#include <utility>
#include <sstream>
#include <iostream>
#include <iomanip>
#include <cstdio>
#include <cmath>
#include <cstdlib>
#include <cctype>
#include <string>
#include <cstring>
#include <ctime>
#include <cassert>
#include <cstdarg>

using std::cout;
using std::vector;
using std::cerr;
using std::string;

#include <Windows.h>
#include <conio.h>
#include <crtdbg.h>
#include <process.h>

#define PI (2*acos(0.0f))


class ImageUtils
{
public:
	static IplImage* clipHeadImage(IplImage* src, HeadPose pose);
	static void rotateImage( IplImage *target, double angle, cv::Point2f center);
	static void adjustGamma( IplImage *src, IplImage* dst, double gamma);
	static void adjustBrightnessAndContrast(IplImage* src, IplImage* dst, int bparm, int cparam);
	static void adjustSaturation(IplImage* src, IplImage* dst, unsigned char sparam);
	static bool drawAlphaImage(IplImage* bgImg, IplImage* rgbImg, IplImage* alphaImg, int x, int y, int width, int height);
};

class FileUtils
{
public:
	static void DeleteAllTmpImages();
};

class SerialAdapter{
private:
	HANDLE hPort;
	COMMCONFIG comcfg;
	unsigned long ncomcfg;
	char comName[256];

	bool f_start;
	bool f_loop;
	int wait;

	HANDLE hThread;
	HANDLE hMutex;
	std::string input;
	std::queue <string> outque;
	virtual void SetupTimeout(COMMTIMEOUTS* timeout);
	virtual void SetupCommConfig(COMMCONFIG* comcfg);
	static unsigned int CALLBACK Launch(void*);
	virtual void Loop();

public:
	SerialAdapter();
	virtual ~SerialAdapter();

	virtual bool Init();
	virtual void Close();
	virtual void Start();
	virtual void Stop();
	virtual bool IsActive();
	virtual bool IsStart();
	virtual void SetWait(int wait);

	virtual void Write(string);
	virtual void Read(string*);
	virtual int GetReadQueueCount();
	virtual int GetWriteQueueCount();
	virtual void SetupCommState(DCB *dcb);
};