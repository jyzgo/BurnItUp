using UnityEngine;
using System.Collections;
using MTUnity.Actions;
using MTUnity.Utils;

namespace MTUnity.Effects
{
	
	/// <summary>
	/// Trail.
	/// </summary>
	public class Trail : MonoBehaviour
	{
		[SerializeField]
		private Transform trail;

		private Vector3 lastPos;
		private Vector3 curPos;

		void Start ()
		{
			lastPos = Vector3.zero;
			curPos = Vector3.zero;
		}

		void Update ()
		{
			if (trail == null)
				return; 
			curPos = trail.localPosition;
			if (curPos != lastPos) {
				Vector3 dis = new Vector3 (curPos.x - lastPos.x, curPos.y - lastPos.y, 0);
				float r = Mathf.Atan2 (dis.y, dis.x) * 180 / Mathf.PI;
				trail.localRotation = Quaternion.Euler (new Vector3 (0, 0, r));
			}
			lastPos = curPos;
		}

		#region curve

		IEnumerator CurveAction (Vector3 startPosition, Vector3 endPosition, Vector3 bending, float delayTime, float timeToTravel)
		{
			yield return new WaitForSeconds (delayTime);
			var timeStamp = Time.time;
			while (Time.time < timeStamp + timeToTravel) {
				var currentPos = Vector3.Lerp (startPosition, endPosition, (Time.time - timeStamp) / timeToTravel);
			
				currentPos.x += bending.x * Mathf.Sin (Mathf.Clamp01 ((Time.time - timeStamp) / timeToTravel) * Mathf.PI);
				currentPos.y += bending.y * Mathf.Sin (Mathf.Clamp01 ((Time.time - timeStamp) / timeToTravel) * Mathf.PI);
				currentPos.z += bending.z * Mathf.Sin (Mathf.Clamp01 ((Time.time - timeStamp) / timeToTravel) * Mathf.PI);
			
				transform.localPosition = currentPos;
			
				yield return  null;
			}
		}

		#endregion curve


		void RunTo (Vector3 startPos, Vector3 endPos, float delayTime, float showTime, TrailConfig config)
		{
			StartCoroutine (CurveAction (startPos, endPos, Vector3.up, delayTime, showTime));
			Destroy (gameObject, delayTime + showTime);
			return; //----------------------curve---------------

			int minAngle = config.MinAngle;
			int maxAngle = config.MaxAngle;
			float minDist = config.MinDistRatio;
			float maxDist = config.MaxDistRatio;

			int angle = UnityEngine.Random.Range (minAngle, maxAngle);
			if (MTRandom.GetRandomInt (1, 2) > 1) {
				angle = -angle;
			}

			Vector3 dis = new Vector3 (endPos.x - startPos.x, endPos.y - startPos.y, 0);
			float r = Mathf.Atan2 (dis.y, dis.x) * 180 / Mathf.PI;
			angle += (int)r;

			//计算两点之间随机值
			float dist = UnityEngine.Random.Range (minDist, maxDist);
			Vector3 middlePos = Vector3.Lerp (startPos, endPos, dist);
			dist = Vector3.Distance (startPos, middlePos);

//		middlePos = Vector3.RotateTowards

			float distx = Mathf.Cos (angle) * dist;
			float disty = Mathf.Sin (angle) * dist;

			Vector3 c1 = new Vector3 (distx + startPos.x, disty + startPos.y, 0);

//		c1 = new Vector3 (c1.x / 100, c1.y / 100, 0);
//		endPos = new Vector3 (endPos.x / 100, endPos.y / 100, 0);

			MTBezierConfig bconfig = new MTBezierConfig ();
			bconfig.ControlPoint1 = c1;
			bconfig.ControlPoint2 = c1;
			bconfig.EndPosition = endPos;

			MTBezierTo action = new MTBezierTo (showTime, bconfig);
//		trail.gameObject.RunAction (action);
			trail.gameObject.RunActions (new MTDelayTime (delayTime), action, new MTDestroy ());



//		MTMoveTo action = new MTMoveTo (showTime, endPos);
//		trail.gameObject.RunAction (action);
		}

		Vector3 interpolatePoint (Vector3 startPos, Vector3 endPos, float ratio)
		{
			Vector3 pos = new Vector3 ();
			pos.x = startPos.x + (endPos.x - startPos.x) * ratio;
			pos.y = startPos.y + (endPos.y - startPos.y) * ratio;
			pos.z = startPos.z + (endPos.z - startPos.z) * ratio;
			return pos;
		}

		public static void CreateRender (Transform parent, Vector3 startPos, Vector3 endPos, GameObject go)
		{
			go.transform.localPosition = startPos;
			TransformUtil.AddChild (parent, go.transform);
			Trail trail = go.GetComponent<Trail> ();
			if (trail == null) {
				trail = go.AddComponent<Trail> ();
			}
			trail.RunTo (startPos, endPos, 0, 0.6f, new TrailConfig (30, 100, 0.3f, 1f));
		}

		public static void CreateRender (Transform parent, Vector3 startPos, Vector3 endPos, float delayTime, float showTime, TrailConfig config, GameObject go)
		{
			go.transform.localPosition = startPos;
			TransformUtil.AddChild (parent, go.transform);
			Trail trail = go.GetComponent<Trail> ();
			if (trail == null) {
				trail = go.AddComponent<Trail> ();
			}
			trail.RunTo (startPos, endPos, delayTime, showTime, config);
		}

	}

	public struct TrailConfig
	{
		public int MinAngle;
		public int MaxAngle;
		public float MinDistRatio;
		public float MaxDistRatio;

		public TrailConfig (int MinAngle, int MaxAngle, float MinDistRatio, float MaxDistRatio)
		{
			this.MinAngle = MinAngle;
			this.MaxAngle = MaxAngle;
			this.MinDistRatio = MinDistRatio;
			this.MaxDistRatio = MaxDistRatio;
		}

	}

}