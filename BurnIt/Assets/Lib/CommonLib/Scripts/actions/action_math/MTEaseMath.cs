using System;

namespace MTUnity.Actions
{
	internal static class MTEaseMath
	{
		// Sine Ease
		internal static float SineIn (float time)
		{
			return -1f * (float)Math.Cos (time * MTMathHelper.Pi_2) + 1f;
		}

		internal static float SineOut (float time)
		{
			return (float)Math.Sin (time * MTMathHelper.Pi_2);
		}

		internal static float SineInOut (float time)
		{
			return -0.5f * ((float)Math.Cos ((float)Math.PI * time) - 1f);
		}

		// Quad Ease
		internal static float QuadIn (float time)
		{
			return time * time;
		}

		internal static float QuadOut (float time)
		{
			return -1f * time * (time - 2f);
		}

		internal static float QuadInOut (float time)
		{
			time = time * 2f;
			if (time < 1f)
				return 0.5f * time * time;
			--time;
			return -0.5f * (time * (time - 2f) - 1f);
		}

		// Cubic Ease
		internal static float CubicIn (float time)
		{
			return time * time * time;
		}

		internal static float CubicOut (float time)
		{
			time -= 1f;
			return (time * time * time + 1f);
		}

		internal static float CubicInOut (float time)
		{
			time = time * 2f;
			if (time < 1f)
				return 0.5f * time * time * time;
			time -= 2f;
			return 0.5f * (time * time * time + 2f);
		}

		// Quart Ease
		internal static float QuartIn (float time)
		{
			return time * time * time * time;
		}

		internal static float QuartOut (float time)
		{
			time -= 1f;
			return -(time * time * time * time - 1f);
		}

		internal static float QuartInOut (float time)
		{
			time = time * 2f;
			if (time < 1f)
				return 0.5f * time * time * time * time;
			time -= 2f;
			return -0.5f * (time * time * time * time - 2f);
		}

		// Quint Ease
		internal static float QuintIn (float time)
		{
			return time * time * time * time * time;
		}

		internal static float QuintOut (float time)
		{
			time -= 1f;
			return (time * time * time * time * time + 1f);
		}

		internal static float QuintInOut (float time)
		{
			time = time * 2f;
			if (time < 1f)
				return 0.5f * time * time * time * time * time;
			time -= 2f;
			return 0.5f * (time * time * time * time * time + 2f);
		}

		// Expo Ease
		internal static float ExpoIn (float time)
		{
			return time == 0f ? 0f : (float)Math.Pow (2f, 10f * (time / 1f - 1f)) - 1f * 0.001f;
		}

		internal static float ExpoOut (float time)
		{
			return time == 1f ? 1f : (-(float)Math.Pow (2f, -10f * time / 1f) + 1f);
		}

		internal static float ExpoInOut (float time)
		{
			time /= 0.5f;
			if (time < 1) {
				return 0.5f * (float)Math.Pow (2f, 10f * (time - 1f));
			} else {
				return 0.5f * (-(float)Math.Pow (2f, -10f * (time - 1f)) + 2f);
			}
		}

		// Circ Ease
		internal static float CircIn (float time)
		{
			return -1f * ((float)Math.Sqrt (1f - time * time) - 1f);
		}

		internal static float CircOut (float time)
		{
			time = time - 1f;
			return (float)Math.Sqrt (1f - time * time);
		}

		internal static float CircInOut (float time)
		{
			time = time * 2f;
			if (time < 1f)
				return -0.5f * ((float)Math.Sqrt (1f - time * time) - 1f);
			time -= 2f;
			return 0.5f * ((float)Math.Sqrt (1f - time * time) + 1f);
		}

		// Elastic Ease
		internal static float ElasticIn (float time, float period)
		{
			if (time == 0 || time == 1) {
				return time;
			} else {
				float s = period / 4;
				time = time - 1;
				return -(float)(Math.Pow (2, 10 * time) * Math.Sin ((time - s) * MTMathHelper.Pi * 2.0f / period));
			}
		}

		internal static float ElasticOut (float time, float period)
		{
			if (time == 0 || time == 1) {
				return time;
			} else {
				float s = period / 4;
				return (float)(Math.Pow (2, -10 * time) * Math.Sin ((time - s) * MTMathHelper.Pi * 2f / period) + 1);
			}
		}

		internal static float ElasticInOut (float time, float period)
		{
			if (time == 0 || time == 1) {
				return time;
			} else {
				time = time * 2;
				if (period == 0) {
					period = 0.3f * 1.5f;
				}

				float s = period / 4;

				time = time - 1;
				if (time < 0) {
					return (float)(-0.5f * Math.Pow (2, 10 * time) * Math.Sin ((time - s) * MTMathHelper.TwoPi / period));
				} else {
					return (float)(Math.Pow (2, -10 * time) * Math.Sin ((time - s) * MTMathHelper.TwoPi / period) * 0.5f + 1);
				}
			}
		}

		// Back Ease
		internal static float BackIn (float time)
		{
			const float overshoot = 1.70158f;
            
			return time * time * ((overshoot + 1) * time - overshoot);
		}

		internal static float BackOut (float time)
		{
			const float overshoot = 1.70158f;

			time = time - 1;
			return time * time * ((overshoot + 1) * time + overshoot) + 1;
		}

		internal static float BackInOut (float time)
		{
			const float overshoot = 1.70158f * 1.525f;

			time = time * 2;
			if (time < 1) {
				return (time * time * ((overshoot + 1) * time - overshoot)) / 2;
			} else {
				time = time - 2;
				return (time * time * ((overshoot + 1) * time + overshoot)) / 2 + 1;
			}
		}

		// Bounce Ease
		internal static float BounceTime (float time)
		{
			if (time < 1 / 2.75) {
				return 7.5625f * time * time;
			} else if (time < 2 / 2.75) {
				time -= 1.5f / 2.75f;
				return 7.5625f * time * time + 0.75f;
			} else if (time < 2.5 / 2.75) {
				time -= 2.25f / 2.75f;
				return 7.5625f * time * time + 0.9375f;
			}

			time -= 2.625f / 2.75f;
			return 7.5625f * time * time + 0.984375f;
		}

		internal static float BounceIn (float time)
		{
			return 1f - BounceTime (1f - time);
		}

		internal static float BounceOut (float time)
		{
			return BounceTime (time);
		}

		internal static float BounceInOut (float time)
		{
			if (time < 0.5f) {
				time = time * 2;
				return (1 - BounceTime (1 - time)) * 0.5f;
			}
			return BounceTime (time * 2 - 1) * 0.5f + 0.5f;
		}

	}
}
