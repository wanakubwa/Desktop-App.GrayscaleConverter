#include "GrayscaleCore.h"
#include <iostream>

namespace GrayscaleCPPDLL
{
	int GrayscaleCPPDLL::GrayscaleCore::TestInitialize(int x, int y)
	{
		std::cout << "Called function from CPP DLL" << std::endl;
		return x + y;
	}
}

