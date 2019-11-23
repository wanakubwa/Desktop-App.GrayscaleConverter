#pragma once
#include "ManagedObject.h"

// Path to header from cpp project.
#include "../CPPCore/CPPCore.h"

/// The ilusion of existing class using to connection between them.
/// C# thinks that use class from this dll file but using original class wraped by this managed class.
namespace GrayscaleCppManager
{
	public ref class GrayscaleConverterCpp : public ManagedObject<CppGrayscaleCore::GrayscaleConverterCpp>
	{
	public:
		GrayscaleConverterCpp();

		// All function must be declared there but using c# data types name.
		// I.e Entity(int^ array);
		// int^ must be converted (to pointer to first element in array) by call conversion function declared in manager class.

		int TestInitialize(int x, int y);
	};
}