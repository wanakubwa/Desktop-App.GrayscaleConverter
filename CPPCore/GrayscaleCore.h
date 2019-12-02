#pragma once
#include <dvec.h>

namespace CppGrayscaleCore
{
	class GrayscaleConverterCpp
	{
	public:
		// All logical functions converting using for grayscale must by declare here.
		// Only functions no data conversions methods.
		void MakeGrayScaleAtOneRegisterCpp(unsigned char *src, int size);
	private:
		__m128i _mm_mulif_epi16(__m128i inputIntVect, __m128 InputFloatVect);
	};
}