using System;

using UnityEngine;

namespace MTUnity.Actions
{
	public class MTRotateMoveRoundTo : MTFiniteTimeAction
	{

		#region Constructors

		public new float Duration{ get; private set; }

		public float Radius{ get; private set; }

		public float RadiusOri{ get; private set; }

		public float StartAngle{ get; private set; }

		public float AngleDuration{ get; private set; }

		public Vector3 Center{ get; private set; }

		public MTRotateMoveRoundTo (float duration, float radius, float radiusOri, Vector3 center, float startAngle, float angleDuration) : base (duration)
		{
			Duration = duration;
			Center = center;
			Radius = radius;
			RadiusOri = radiusOri;
			StartAngle = startAngle;
			AngleDuration = angleDuration;
		}

		#endregion Constructors

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTRotateMoveRoundToState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			throw new NotImplementedException ();
		}
	}


	public class MTRotateMoveRoundToState : MTFiniteTimeActionState
	{

		public MTRotateMoveRoundToState (MTRotateMoveRoundTo action, GameObject target)
			: base (action, target)
		{ 
			FromAngle = action.StartAngle;
			ToAngle = action.AngleDuration;
			Radius = action.Radius;
			RadiusOri = action.RadiusOri;
			RadiusDis = RadiusOri - Radius;
			InTime = action.Duration;
			Center = action.Center;
		}

		Vector3 Center;
		float FromAngle;
		float ToAngle;
		float Radius;
		float RadiusOri;
		float RadiusDis;
		float InTime;
		float curTime = 0f;

		public override void Update (float time)
		{
			if (Target != null) {
				Vector3 pos = Vector3.zero;
				if (curTime / InTime < 0.85f)
					RadiusOri = RadiusOri - RadiusDis * Time.deltaTime / InTime * 1f / 0.85f;
				else
					RadiusOri = RadiusOri + RadiusDis * Time.deltaTime / InTime * 1.5f;
				pos.x = Mathf.Cos ((curTime / InTime * ToAngle + FromAngle) * Mathf.Deg2Rad) * RadiusOri + Center.x;
				pos.y = Mathf.Sin ((curTime / InTime * ToAngle + FromAngle) * Mathf.Deg2Rad) * RadiusOri + Center.y;

				Target.transform.localPosition = pos;

				Vector3 dir = pos - Center;

//				Target.transform.localRotation = Quaternion.Euler (0, 0, Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg);
				Target.transform.rotation = Quaternion.RotateTowards (Target.transform.localRotation,  Quaternion.Euler (0, 0, Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg), 0.3f);
//				Target.transform.localRotation = Quaternion.Euler (0, 0, Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg);

				curTime += Time.deltaTime;
			}
		}

	}
}