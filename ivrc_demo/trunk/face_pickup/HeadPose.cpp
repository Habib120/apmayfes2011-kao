#include "structure.h"

bool HeadPose::isValueSet() const
{
	return this->z != 0;
}