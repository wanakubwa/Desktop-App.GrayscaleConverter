#pragma once
#include "ManagedObject.h"

// Path to header from cpp project.
#include "../CPPCore/CPPCore.h"

namespace ManagedCPP_DLL
{
	public ref class GrayscaleCore : public ManagedObject<GrayscaleCPPDLL::GrayscaleCore>
	{
	public:
		GrayscaleCore();
		int TestInitialize(int x, int y);
	};
}