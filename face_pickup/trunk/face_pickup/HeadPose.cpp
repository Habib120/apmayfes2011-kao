#include "structure.h"
using namespace structure;

bool HeadPose::isValueSet() const
{
	return this->z != 0;
}