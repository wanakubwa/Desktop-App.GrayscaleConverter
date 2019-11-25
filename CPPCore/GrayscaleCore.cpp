#include "GrayscaleCore.h"
#include <iostream>

namespace CppGrayscaleCore
{
	void GrayscaleConverterCpp::MakeGrayScaleAtOneRegisterCpp(unsigned char * src, int size)
	{
		for (int i = 0; i < size; i += 4) {
			int average = (*(src + i) + *(src + i + 1) + *(src + i + 2)) / 3;
			*(src + i) = average;
			*(src + i + 1) = average;
			*(src + i + 2) = average;
		}
	}
}

