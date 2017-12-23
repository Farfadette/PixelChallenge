using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KaboomOutput
{
	public enum Output
	{
		OUTPUT_NONE = 0,
		OUTPUT_5 = 16,
		OUTPUT_6 = 32,
		OUTPUT_7 = 64,
		OUTPUT_8 = 128,
		OUTPUT_9 = 256,
		OUTPUT_10 = 512,
		OUTPUT_11 = 1024,
		OUTPUT_12 = 2048,
		OUTPUT_13 = 4096,
		OUTPUT_14 = 8192,
		OUTPUT_15 = 16384,
		OUTPUT_16 = 32768,

		OUTPUT_ALL = (OUTPUT_5 | OUTPUT_6 | OUTPUT_7 | OUTPUT_8 | OUTPUT_9 | OUTPUT_10 | OUTPUT_11 | OUTPUT_12 | OUTPUT_13 | OUTPUT_14 | OUTPUT_15 | OUTPUT_16)
    }

	public enum OutputExtended
	{
		OUTPUT_NONE = 0,
		OUTPUT_1 = 1,
		OUTPUT_2 = 2,
	}

	public enum OutputPrize
	{
		OUTPUT_NONE = 0,
		OUTPUT_5 = 5,
		OUTPUT_6 = 6,
		OUTPUT_7 = 7,
		OUTPUT_8 = 8,
		OUTPUT_9 = 9,
		OUTPUT_10 = 10,
		OUTPUT_11 = 11,
		OUTPUT_12 = 12,
		OUTPUT_13 = 13,
		OUTPUT_14 = 14,
		OUTPUT_15 = 15,
		OUTPUT_16 = 16,
	}
}