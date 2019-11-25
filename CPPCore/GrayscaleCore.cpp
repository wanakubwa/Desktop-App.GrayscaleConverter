#include "GrayscaleCore.h"
#include <iostream>

namespace CppGrayscaleCore
{
	// Converting one 128-bit register to grayscale and replace in source as destination.
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

