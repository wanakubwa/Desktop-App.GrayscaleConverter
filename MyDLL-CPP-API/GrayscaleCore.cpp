#include "stdafx.h"
#include "GrayscaleCore.h"

namespace ManagedCPP_DLL 
{
	GrayscaleCore::GrayscaleCore() : ManagedObject(new GrayscaleCPPDLL::GrayscaleCore())
	{
	}
	int GrayscaleCore::TestInitialize(int x, int y)
	{
		// m_instancje contains instance of current using ubject i.e: "GrayscaleCore" from namespace "GrayscaleCPPDLL".
		return _Instance->TestInitialize(x, y);
	}
}

