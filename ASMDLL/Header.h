#pragma once
#ifdef DLLASM_EXPORTS
#define DLLASM_API __declspec(dllexport)
#else
#define DLLASM_API __declspec(dllimport)
#endif

extern "C" DLLASM_API void doRegisterGrayASM(unsigned char *);
