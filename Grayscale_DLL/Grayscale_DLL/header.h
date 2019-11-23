#pragma once
#ifdef DEBUG GRAYSCALE_API_EXPORTS
#define GRAYSCALE_API __declspec(dllexport)
#else
#define GRAYSCALE_API __declspec(dllimport)
#endif // DEBUG GRAYSCALE_API_EXPORTS


// Declare a functions using in dll.
// The declaration of all functions must be there.
extern "C" GRAYSCALE_API void AddNumbers(int a, int b, int *summ);