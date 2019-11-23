#include "stdafx.h"
#include "GrayscaleCore.h"

namespace GrayscaleCppManager 
{
	// Constructor must be always declared.
	GrayscaleConverterCpp::GrayscaleConverterCpp() : ManagedObject(new CppGrayscaleCore::GrayscaleConverterCpp())
	{
	}

	// All functions here using static convertion functions from main Manager class "ManagerObject.h" if its necessary.
	int GrayscaleConverterCpp::TestInitialize(int x, int y)
	{
		// Calling origianl function.
		return _Instance->TestInitialize(x, y);
	}
}

