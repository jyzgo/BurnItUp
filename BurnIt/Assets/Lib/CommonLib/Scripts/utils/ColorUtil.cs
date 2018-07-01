using UnityEngine;

namespace MTUnity.Utils
{
	public class ColorUtil
	{
		public static Color HexToColor (int hexVal)
		{
			byte R = (byte)((hexVal >> 16) & 0xFF);
			byte G = (byte)((hexVal >> 8) & 0xFF);
			byte B = (byte)((hexVal) & 0xFF);
			return new Color32 (R, G, B, 255);
		}
	}
}
