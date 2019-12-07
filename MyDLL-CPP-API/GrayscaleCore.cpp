#include "stdafx.h"
#define WIN32_LEAN_AND_MEAN

// Windows Header Files
#include <windows.h>
#include "GrayscaleCore.h"

namespace GrayscaleCppManager 
{
	// Constructor must be always declared.
	GrayscaleConverterCpp::GrayscaleConverterCpp() : ManagedObject(new CppGrayscaleCore::GrayscaleConverterCpp())
	{
	}

	// All functions here using static convertion functions from main Manager class "ManagerObject.h" if its necessary.
	void GrayscaleConverterCpp::MakeGrayScaleAtOneRegisterCpp(array<Byte>^ src)
	{
		// Calling origianl function.
		return _Instance->MakeGrayScaleAtOneRegisterCpp(byte_array_conversion(src));
	}
}