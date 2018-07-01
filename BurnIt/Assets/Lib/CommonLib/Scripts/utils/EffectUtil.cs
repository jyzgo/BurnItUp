using UnityEngine;
using MTUnity.Actions;

namespace MTUnity.Utils
{
	public class EffectUtil
	{
		/// <summary>
		/// 创建一个弹出果冻效果的Action.
		/// </summary>
		/// <param name="target">目标对象.</param>
		/// <param name="delay">延迟秒数.</param>
		public static MTFiniteTimeAction CreateJellyPopAction (GameObject target, float delay = 0.0f, MTFiniteTimeAction endCall = null)
		{
			//以下效果参考了Flash JollyJam项目中EffectUtil.createJellyPopAction
			Vector3 scale = target.transform.localScale;
			target.transform.localScale = Vector3.zero;

			var act1 = new MTEaseIn (new MTScaleTo (0.2f, 1.1f * scale.x, 1.05f * scale.y, 1f), 1f);
			var act2 = new MTEaseIn (new MTScaleTo (0.0833f, 0.95f * scale.x, 0.85f * scale.y, 1f), 1f);
			var act3 = new MTEaseIn (new MTScaleTo (0.0833f, 1.0f * scale.x, 1.05f * scale.y, 1f), 1f);
			var act4 = new MTEaseIn (new MTScaleTo (0.125f, 0.95f * scale.x, 0.95f * scale.y, 1f), 1f);
			var act5 = new MTEaseIn (new MTScaleTo (0.167f, 1.0f * scale.x, 1.0f * scale.y, 1f), 1f);

			if (endCall == null) {
				return new MTSequence (new MTDelayTime (delay), act1, act2, act3, act4, act5);
			} else {
				return new MTSequence (new MTDelayTime (delay), act1, act2, act3, act4, act5, endCall);
			}
		}

		/// <summary>
		/// 创建一个果冻效果的Action.（非弹窗）
		/// </summary>
		/// <param name="target">目标对象.</param>
		/// <param name="delay">延迟秒数.</param>
		public static MTFiniteTimeAction CreateJellyAction (GameObject target, float delay = 0.0f, float timeScale = 1.0f, MTFiniteTimeAction endCall = null)
		{
			//以下效果参考了Flash JollyJam项目中EffectUtil.createJellyPopAction
			Vector3 scale = target.transform.localScale;

			var act1 = new MTEaseIn (new MTScaleTo (0.2f * timeScale, 1.03f * scale.x, 1f * scale.y, 1f), 1f);
			var act2 = new MTEaseIn (new MTScaleTo (0.0833f * timeScale, 0.97f * scale.x, 0.87f * scale.y, 1f), 1f);
			var act3 = new MTEaseIn (new MTScaleTo (0.0833f * timeScale, 1.0f * scale.x, 1.03f * scale.y, 1f), 1f);
			var act4 = new MTEaseIn (new MTScaleTo (0.125f * timeScale, 0.97f * scale.x, 0.97f * scale.y, 1f), 1f);
			var act5 = new MTEaseIn (new MTScaleTo (0.167f * timeScale, 1.0f * scale.x, 1.0f * scale.y, 1f), 1f);

			if (endCall == null) {
				return new MTSequence (new MTDelayTime (delay), act1, act2, act3, act4, act5);
			} else {
				return new MTSequence (new MTDelayTime (delay), act1, act2, act3, act4, act5, endCall);
			}
		}

		/// <summary>
		/// 创建一个消失果冻效果的Action.
		/// </summary>
		/// <param name="target">目标对象.</param>
		/// <param name="delay">延迟秒数.</param>
		public static MTFiniteTimeAction CreateJellyPopOutAction (GameObject target, float delay = 0.0f)
		{
			var act1 = new MTScaleTo (0.1f, 0.1f, 0f, 0f);

			var sequence = new MTSequence (new MTDelayTime (delay), act1);
			return sequence;
		}

		/// <summary>
		/// 开始呼吸效果.
		/// </summary>
		/// <returns>The breathe effect.</returns>
		/// <param name="target">目标.</param>
		/// <param name="scale1">Scale1.</param>
		/// <param name="scale2">Scale2.</param>
		/// <param name="duration">一呼或者一吸的持续时间（单位：秒数），默认为0.9秒.</param>
		public static MTFiniteTimeAction CreateBreatheEffect (GameObject target, float scale1, float scale2, float duration = 0.9f)
		{
			Vector3 scale = target.transform.localScale;

			var sx = scale.x;
			var sy = scale.y;

			var sx1 = scale1 * sx;
			var sx2 = scale2 * sx;
			var sy1 = scale1 * sy;
			var sy2 = scale2 * sy;

			var act1 = new MTEaseIn (new MTScaleTo (duration, sx1, sy2, scale.z), 1f);
			var act2 = new MTEaseIn (new MTScaleTo (duration, sx2, sy1, scale.z), 1f);

			var sequence = new MTSequence (act1, act2);
			var repeat = new MTRepeatForever (sequence);

			return repeat;
		}

		/// <summary>
		/// 生成贝塞尔曲线的控制点
		/// </summary>
		/// <param name="startPos">Start position.</param>
		/// <param name="endPos">End position.</param>
		/// <param name="config">Config.</param>
		/// <param name="middlePos1">Middle pos1.</param>
		/// <param name="middlePos2">Middle pos2.</param>
		public static void BesizeControlPos(Vector3 startPos, Vector3 endPos, BesizeControlConfig config, out Vector3 middlePos1, out Vector3 middlePos2, bool left = true)
		{
			Vector3 originalStartPos = startPos;

			int minAngle = config.MinAngle;
			int maxAngle = config.MaxAngle;
			float minDist = config.MinDistRatio;
			float maxDist = config.MaxDistRatio;

			int angle = UnityEngine.Random.Range (minAngle, maxAngle);
			if (!left) {
				angle = -angle;
			}

			endPos = endPos - startPos;
			endPos.z = 0;
			startPos = Vector3.zero;

			Vector3 dis = new Vector3 (endPos.x - startPos.x, endPos.y - startPos.y, 0);
			float r = Mathf.Atan2 (dis.y, dis.x) * 180 / Mathf.PI;

			angle += (int)r;

			//计算两点之间随机值
			float dist = UnityEngine.Random.Range (minDist, maxDist);
			bool negativeFlag = false;
			if (dist < 0) {
				dist = Mathf.Abs (dist);
				negativeFlag = true;
			}
			Vector3 middlePos = Vector3.Lerp (startPos, endPos, dist);
			dist = Vector3.Distance (startPos, middlePos);

			float distx = Mathf.Cos (Mathf.Deg2Rad * angle) * dist;
			float disty = Mathf.Sin (Mathf.Deg2Rad * angle) * dist;
			if (negativeFlag) {
				distx = -distx;
				disty = -disty;
			}

			middlePos1 = new Vector3 (distx, disty, 0) + originalStartPos;
			middlePos1.z = 0;

			middlePos2 = middlePos1;
		}

		/// <summary>
		/// 创建Besize曲线的Action
		/// </summary>
		/// <param name="targetTF">Target T.</param>
		/// <param name="startPos">Start position.</param>
		/// <param name="endPos">End position.</param>
		/// <param name="showTime">Show time.</param>
		/// <param name="config">Config.</param>
		public static MTBezierTo CreateBesizeAction (Transform targetTF, Vector3 startPos, Vector3 endPos, float showTime, BesizeControlConfig config)
		{
			Vector3 controller1;
			EffectUtil.BesizeControlPos (startPos, endPos, config, out controller1, out controller1, MTRandom.GetRandomInt (1, 2) > 1);
			controller1.z = 0;

			startPos = targetTF.InverseTransformPoint (startPos);
			startPos.z = 0;
			endPos = targetTF.InverseTransformPoint (endPos);
			endPos.z = 0;
			controller1 = targetTF.InverseTransformPoint (controller1);
			controller1.z = 0;

			MTBezierConfig bconfig = new MTBezierConfig ();
			bconfig.ControlPoint1 = controller1;
			bconfig.ControlPoint2 = controller1;
			bconfig.EndPosition = endPos;

			return new MTBezierTo (showTime, bconfig);
		}

		/// <summary>
		/// 创建Besize曲线的Action
		/// </summary>
		/// <param name="targetTF">Target T.</param>
		/// <param name="startPos">Start position.</param>
		/// <param name="endPos">End position.</param>
		/// <param name="showTime">Show time.</param>
		/// <param name="config">Config.</param>
		public static MTBezierTo CreateBesizeAction (Transform targetTF, Vector3 startPos, Vector3 endPos, float showTime, BesizeControlConfig config, BesizeControlConfig config2)
		{
			bool left = MTRandom.GetRandomInt (1, 2) > 1;

			Vector3 controller1;
			EffectUtil.BesizeControlPos (startPos, endPos, config, out controller1, out controller1, left);
			controller1.z = 0;

			Vector3 controller2;
			EffectUtil.BesizeControlPos (startPos, endPos, config2, out controller2, out controller2, left);
			controller2.z = 0;

			startPos = targetTF.InverseTransformPoint (startPos);
			startPos.z = 0;
			endPos = targetTF.InverseTransformPoint (endPos);
			endPos.z = 0;
			controller1 = targetTF.InverseTransformPoint (controller1);
			controller1.z = 0;
			controller2 = targetTF.InverseTransformPoint (controller2);
			controller2.z = 0;

			MTBezierConfig bconfig = new MTBezierConfig ();
			bconfig.ControlPoint1 = controller1;
			bconfig.ControlPoint2 = controller2;
			bconfig.EndPosition = endPos;

			return new MTBezierTo (showTime, bconfig);
		}

	}

	public struct BesizeControlConfig
	{
		public int MinAngle;
		public int MaxAngle;
		public float MinDistRatio;
		public float MaxDistRatio;

		public BesizeControlConfig (int MinAngle, int MaxAngle, float MinDistRatio, float MaxDistRatio)
		{
			this.MinAngle = MinAngle;
			this.MaxAngle = MaxAngle;
			this.MinDistRatio = MinDistRatio;
			this.MaxDistRatio = MaxDistRatio;
		}

	}

}