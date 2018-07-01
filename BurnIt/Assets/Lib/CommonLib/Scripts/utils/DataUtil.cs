using System;
using System.Collections.Generic;

namespace MTUnity.Utils
{
	
	public class DataUtil
	{
		
		/// <summary>
		/// 根据字符串获取一个枚举的值
		/// </summary>
		/// <returns>The enum.</returns>
		/// <param name="type">Type.</param>
		/// <param name="value">Value.</param>
		/// <param name="ignoreCase">If set to <c>true</c> ignore case.</param>
		public static object GetEnum (Type type, string value, bool ignoreCase = false)
		{
			int findIndex = -1;
			string[] names = Enum.GetNames (type);
			for (int i = 0; i < names.Length; i++) {
				if (ignoreCase) {
					if (value.ToLower () == names [i].ToLower ()) {
						findIndex = i;
						break;
					}
				} else {
					if (value == names [i]) {
						findIndex = i;
						break;
					}
				}
			}
			if (findIndex >= 0) {
				return Enum.Parse (type, Enum.GetName (type, findIndex));
			}
			return null;
		}

		public static void ShuffleArray<T> (T[] arr, int begin = 0, int end = 16777215)
		{
			if (arr == null || arr.Length < 2)
				return;

			if (begin >= arr.Length)
				return;
			if (end >= arr.Length) {
				end = arr.Length - 1;
			}

			for (int i = begin; i <= end; i++) {
				int j = UnityEngine.Random.Range (begin, end);
				T tmp = arr [i];
				arr [i] = arr [j];
				arr [j] = tmp;
			}
		}

		public static void ShuffleList<T> (List<T> list, int begin = 0, int end = int.MaxValue)
		{
			if (list == null || list.Count < 2)
				return;

			if (begin >= list.Count)
				return;
			if (end >= list.Count) {
				end = list.Count - 1;
			}

			for (int i = begin; i <= end; i++) {
				int j = UnityEngine.Random.Range (begin, end);
				T tmp = list [i];
				list [i] = list [j];
				list [j] = tmp;
			}
		}

		public static T ParseEnum<T>(string value)
		{
			return (T) Enum.Parse(typeof(T), value, true);
		}

	}
	
}