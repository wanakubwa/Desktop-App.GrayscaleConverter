// Grayscale_DLL.cpp : Definiuje eksportowane funkcje dla aplikacji DLL.
//

#include "stdafx.h"
#include "header.h"

void AddNumbers(int a, int b, int * summ)
{
	(*summ) = a + b;
}