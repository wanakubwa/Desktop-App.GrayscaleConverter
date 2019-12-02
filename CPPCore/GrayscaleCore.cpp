#include "GrayscaleCore.h"
#include <iostream>

namespace CppGrayscaleCore
{
	// Converting one 128-bit register to grayscale and replace in source as destination. BGRA format
	void GrayscaleConverterCpp::MakeGrayScaleAtOneRegisterCpp(unsigned char * src, int size)
	{
		// Start index of src tab.
		int srcIndex = 0;

		// vecSrc - source vector (BGRA...)
		// Load data from src into 128-bit register starting from adress at pointer src. (From 0 index so load all 16 elements x 8bit).
		__m128i vecSrc = _mm_loadu_si128((__m128i*) &src[srcIndex]);

		// Define r,g,b factors used to calculate the average.
		const __m128i r_coef = _mm_set1_epi16((short)(0.2989*32768.0 + 0.5));  //8 coefficients - R scale factor.
		const __m128i g_coef = _mm_set1_epi16((short)(0.5870*32768.0 + 0.5));  //8 coefficients - G scale factor.
		const __m128i b_coef = _mm_set1_epi16((short)(0.1140*32768.0 + 0.5));  //8 coefficients - B scale factor.

		// Shuffle to configuration A0A1A2A3_R0R1R2R3_G0G1G2G3_B0B1B2B3
		// Not revers so mask is read from left (Lo) to right (Hi). And counting from righ in srcVect (Lo).
		__m128i shuffleMask = _mm_set_epi8(15, 11, 7, 3, 14, 10, 6, 2, 13, 9, 5, 1, 12, 8, 4, 0);
		__m128i AAAA_R0RRR_G0GGG_B0BBB = _mm_shuffle_epi8(vecSrc, shuffleMask);

		// Put BBBB in lower part.
		__m128i B0_XXX = _mm_slli_si128(AAAA_R0RRR_G0GGG_B0BBB, 12);
		__m128i XXX_B0 = _mm_srli_si128(B0_XXX, 12);

		// Put GGGG in Lower part.
		__m128i G0_B_XX = _mm_slli_si128(AAAA_R0RRR_G0GGG_B0BBB, 8);
		__m128i XXX_G0 = _mm_srli_si128(G0_B_XX, 12);

		// Put RRRR in Lower part.
		__m128i R0_G_XX = _mm_slli_si128(AAAA_R0RRR_G0GGG_B0BBB, 4);
		__m128i XXX_R0 = _mm_srli_si128(R0_G_XX, 12);

		// Unpack uint8 elements to uint16 elements.
		__m128i B0BBB = _mm_cvtepu8_epi16(XXX_B0);
		__m128i G0GGG = _mm_cvtepu8_epi16(XXX_G0);
		__m128i R0RRR = _mm_cvtepu8_epi16(XXX_R0);

		// Multiply each channel by separate factor.
		__m128i B0BBB_mul = _mm_mulhrs_epi16(B0BBB, b_coef);
		__m128i G0GGG_mul = _mm_mulhrs_epi16(G0GGG, g_coef);
		__m128i R0RRR_mul = _mm_mulhrs_epi16(R0RRR, r_coef);

		// Add R+B+G = Gray.
		__m128i BGR_gray = _mm_add_epi16(_mm_add_epi16(B0BBB_mul, G0GGG_mul), R0RRR_mul);

		// Shuffle to set each channel to oryginal destination position.
		__m128i grayMsk = _mm_setr_epi8(0, 0, 0, 0, 2, 2, 2, 2, 4, 4, 4, 4, 6, 6, 6, 6);
		__m128i vectGray = _mm_shuffle_epi8(BGR_gray, grayMsk);

		// Put result vector into array to return as src pointer.
		_mm_store_si128((__m128i*)&src[srcIndex], vectGray);
	}
}

