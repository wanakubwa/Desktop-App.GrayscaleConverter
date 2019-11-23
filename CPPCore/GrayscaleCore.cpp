#include "GrayscaleCore.h"
#include <iostream>

namespace CppGrayscaleCore
{
	int CppGrayscaleCore::GrayscaleConverterCpp::TestInitialize(int x, int y)
	{
		std::cout << "Called function from CPP DLL" << std::endl;
		return x + y;
	}
}

