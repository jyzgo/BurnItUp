
namespace MTUnity.Utils
{
	
	public class StringUtil
	{
		
		/// <summary>
		/// 尝试将字符串转化为整形
		/// </summary>
		/// <returns>The parse string.</returns>
		/// <param name="value">Value.</param>
		/// <param name="def">转化不成功的默认值</param>
		public static int TryParseString (string value, int def)
		{
			int i = 0;
			if (int.TryParse (value, out i)) {
				return i;
			} else {
				return def;
			}
		}

	}
	
}