using System;
using MTUnity.Actions;
using UnityEngine;
using System.Collections.Generic;

namespace MTUnity.Utils
{
	public class MathUtil
	{
		public MathUtil ()
		{
		}

		/// <summary>
		/// 保持value在指定的最大值和最小值之间
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="minimum">Minimum.</param>
		/// <param name="maximum">Maximum.</param>
		public static double Clamp (double value, double minimum, double maximum)
		{
			if (minimum > maximum) {
				Debug.LogWarning ("MathUtil::Clamp: minimum should be smaller than maximum.");
			}
			return Math.Min (maximum, Math.Max (minimum, value));
		}

		/// <summary>
		/// 保持value在指定的a值和b值之间
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="value">Value.</param>
		public static double Between (double a, double b, double value)
		{
			if (a > b) {
				double temp = a;
				a = b;
				b = temp;
			}
			return Clamp (value, a, b);
		}

		/// <summary>
		/// <para>如果value超出a、b大值，value取小值，反之亦然</para>
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="value">Value.</param>
		public static double BetweenLoop (int a, int b, int value)
		{
			//a小 b大
			if (a > b) {
				int temp = a;
				a = b;
				b = temp;
			}
			if (value > b) {
				value = a;
			}
			if (value < a) {
				value = b;
			}
			return value;
		}

		/// <summary>
		/// 按权重获取随机整数
		/// </summary>
		/// <param name="weights">Weights.</param>
		public static int RandomByWeight (int[] weights)
		{
			int sum = 0;
			for (int i = 0; i < weights.Length; i++) {
				sum += weights [i];
			}
			int rnd = MTRandom.Next (sum);

			int cumulate = 0;
			for (int i = 0; i < weights.Length; i++) {
				if (cumulate > rnd) {
					return i - 1;
				}
				cumulate += weights [i];
			}
			return weights.Length - 1;
        }

        /// <summary>
        /// 按权重获取随机整数
        /// </summary>
        /// <param name="weights">Weights.</param>
        public static int RandomByWeight (List<int> weights)
        {
            int sum = 0;
            for (int i = 0; i < weights.Count; i++) {
                sum += weights [i];
            }
            int rnd = MTRandom.Next (sum);

            int cumulate = 0;
            for (int i = 0; i < weights.Count; i++) {
                if (cumulate > rnd) {
                    return i - 1;
                }
                cumulate += weights [i];
            }
            return weights.Count - 1;
        }

		/// <summary>
		/// 获取不受控制的Lerp（Vector3.LerpUnclamped不被低版本支持）
		/// </summary>
		public static Vector3 Lerp (Vector3 a, Vector3 b, float t)
		{
			if (t < 0) {
				t = -t;
				b = new Vector3 (a.x - (b.x - a.x), a.y - (b.y - a.y), a.z - (b.z - a.z));
			}
			if (t > 2) {
				t = t % 2;
				if (t == 0)
					t = 1;
			}
			if (t > 1) {
				t = t - 1;
				Vector3 temp = b;
				b = new Vector3 (b.x + b.x - a.x, b.y + b.y - a.y, b.z + b.z - a.z);
				a = temp;
			}
			if (t > 1) {
				return Lerp (a, b, t);
			}
			return Vector3.Lerp (a, b, t);
		}

        public static float AngleBetweenVectors(Vector3 one, Vector3 two)
        {
            float a = one.sqrMagnitude;
            float b = two.sqrMagnitude;
            if (a > 0.0f && b > 0.0f)
            {
                float angle = Mathf.Acos(Vector3.Dot(one, two) / Mathf.Sqrt(a * b)) * 180.0f / Mathf.PI;

                Vector3 cross = Vector3.Cross(one, two);
                float sign = cross.y;
                if (sign < 0.0f)
                    return -angle;
                else
                    return angle;
            }
            return 0.0f;
        }

    }
	
}