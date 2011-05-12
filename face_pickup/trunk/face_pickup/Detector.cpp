#include "detectors.h"



void Detector::SetDetectAction(void (*action)(std::string))
{
	this->action = action;
}

std::string Detector::Detect(HeadData data)
{
	this->preDetect(data);
	std::string tmp    = this->doDetect(data);
	std::string result = this->postDetect(data, tmp);
	return result;
}

void Detector::preDetect(HeadData data)
{
}

std::string Detector::doDetect(HeadData data)
{
	return "";
}

std::string Detector::postDetect(HeadData data, std::string result)
{
	if (this->action != 0)
	{
		(*this->action)(result);
	}
	return result;
}