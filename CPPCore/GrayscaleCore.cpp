#include "GrayscaleCore.h"
#include <iostream>
#include <dvec.h>

namespace CppGrayscaleCore
{
	// Converting one 128-bit register to grayscale and replace in source as destination.
	void GrayscaleConverterCpp::MakeGrayScaleAtOneRegisterCpp(unsigned char * src, int size)
	{
		// Start index of src tab.
		int srcIndex = 0;

		// vecSrc - source vector (RGBA...), vecR - Red channels (R,0,0,0,R...), vecG - Red channels (0,G,0,0,0,G...), vecB - Red channels (0,0,B,0,0,0,B...) vecA - Alpha channels
		// Load data from src into 128-bit register starting from adress at pointer src. (From 0 index so load all 16 elements x 8bit).
		__m128i vecSrc = _mm_loadu_si128((__m128i*) &src[srcIndex]);

		__m128i maskR = _mm_setr_epi16(1, 0, 0, 0, 1, 0, 0, 0);
		__m128i maskG = _mm_setr_epi16(0, 1, 0, 0, 0, 1, 0, 0);
		__m128i maskB = _mm_setr_epi16(0, 0, 1, 0, 0, 0, 1, 0);
		__m128i maskA = _mm_setr_epi16(0, 0, 0, 1, 0, 0, 0, 1);

		// Creating factors.
		//const __m128i factorR = _mm_set1_epi16((short)(0.29891 * 0x10000));  //8 coefficients - R scale factor.
		//const __m128i factorG = _mm_set1_epi16((short)(0.58661 * 0x10000));  //8 coefficients - G scale factor.
		//const __m128i factorB = _mm_set1_epi16((short)(0.11448 * 0x10000));  //8 coefficients - B scale factor.

		const __m128 factorR = _mm_set1_ps(0.29891);  //8 coefficients - R scale factor.
		const __m128 factorG = _mm_set1_ps(0.58661);  //8 coefficients - G scale factor.
		const __m128 factorB = _mm_set1_ps(0.11448);  //8 coefficients - B scale factor.
		
		__m128i zero = _mm_setzero_si128();

		// Shifting higher part of src register to lower.
		__m128i vectSrcLowInHighPart = _mm_cvtepu8_epi16(vecSrc);
		__m128i vectSrcHighInHighPart = _mm_unpackhi_epi8(vecSrc, zero); 

		//__m128i vectHighToLowShift = _mm_srli_si128(vecSrc, 8);
		//__m128i vectLowInLowShift = vecSrc;

		// Multiply high parts of 16 x uint8 vectors by channels masks and save lower half. Getting each channels separatly (in two parts H and L)
		__m128i vecR_L = _mm_mullo_epi16(vectSrcLowInHighPart, maskR);
		__m128i vecG_L = _mm_mullo_epi16(vectSrcLowInHighPart, maskG);
		__m128i vecB_L = _mm_mullo_epi16(vectSrcLowInHighPart, maskB);
		__m128i vecA_L = _mm_mullo_epi16(vectSrcLowInHighPart, maskA);

		// Multiply lower parts of 16 x uint8 vectors by channels masks and save lower half.
		__m128i vecR_H = _mm_mullo_epi16(vectSrcHighInHighPart, maskR);
		__m128i vecG_H = _mm_mullo_epi16(vectSrcHighInHighPart, maskG);
		__m128i vecB_H = _mm_mullo_epi16(vectSrcHighInHighPart, maskB);
		__m128i vecA_H = _mm_mullo_epi16(vectSrcHighInHighPart, maskA);

		//// Multiply high parts of 16 x uint8 vectors by channels masks and save lower half. Getting each channels separatly (in two parts H and L)
		//__m128i vecR_L = _mm_mulhi_epi16(vectLowInLowShift, maskR);
		//__m128i vecG_L = _mm_mulhi_epi16(vectLowInLowShift, maskG);
		//__m128i vecB_L = _mm_mulhi_epi16(vectLowInLowShift, maskB);
		//__m128i vecA_L = _mm_mulhi_epi16(zero, maskA);

		//// Multiply lower parts of 16 x uint8 vectors by channels masks and save lower half.
		//__m128i vecR_H = _mm_mulhi_epi16(vectHighToLowShift, maskR);
		//__m128i vecG_H = _mm_mulhi_epi16(vectHighToLowShift, maskG);
		//__m128i vecB_H = _mm_mulhi_epi16(vectHighToLowShift, maskB);
		//__m128i vecA_H = _mm_mulhi_epi16(zero, maskA);

		// Lower and high masks using to packing.
		__m128i maskLo = _mm_setr_epi8(0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 14, 12, 10, 8, 6, 4, 2, 0);
		__m128i maskHi = _mm_setr_epi8(14, 12, 10, 8, 6, 4, 2, 0, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80);

		// Packed the High and Lowe part of register into one 16 x 8bit registers of each channels.
		__m128i R = _mm_or_si128(_mm_shuffle_epi8(vecR_L, maskLo), _mm_shuffle_epi8(vecR_H, maskHi));
		__m128i G = _mm_or_si128(_mm_shuffle_epi8(vecG_L, maskLo), _mm_shuffle_epi8(vecG_H, maskHi));
		__m128i B = _mm_or_si128(_mm_shuffle_epi8(vecB_L, maskLo), _mm_shuffle_epi8(vecB_H, maskHi));
		__m128i A = _mm_or_si128(_mm_shuffle_epi8(vecA_L, maskLo), _mm_shuffle_epi8(vecA_H, maskHi));

		// Added all sub vectors to get in result one 128-bit vector with all edited channels.
		__m128i resultVect = _mm_add_epi8(_mm_add_epi8(R, G), _mm_add_epi8(B, A));

		// Put result vector into array to return as src pointer.
		_mm_store_si128((__m128i*)&src[srcIndex], resultVect);
	}
}

