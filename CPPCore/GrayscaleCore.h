#pragma once
namespace CppGrayscaleCore
{
	class GrayscaleConverterCpp
	{
	public:
		// All logical functions converting using for grayscale must by declare here.
		// Only functions no data conversions methods.
		void MakeGrayScaleAtOneRegisterCpp(unsigned char *src, int size);
	};
}