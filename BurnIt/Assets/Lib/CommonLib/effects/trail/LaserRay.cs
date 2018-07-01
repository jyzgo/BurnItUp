using UnityEngine;
using System.Collections;
using MTUnity.Actions;
using MTUnity.Utils;

namespace MTUnity.Effects
{
	/// <summary>
	/// LaserRay
	/// </summary>
	public class LaserRay : MonoBehaviour
	{
		private ParticleSystemRenderer render;

		private float showTime;

		private float passTime;

		private float distance;

		private GameObject target;

		void Update ()
		{
			if (render != null) {
				if (target != null) {
					Vector3 endPos = gameObject.transform.parent.InverseTransformPoint (target.transform.position);

					float angle = Vector2.Angle (Vector2.up, new Vector2 (endPos.x, endPos.y));
					if (endPos.x > 0) {
						angle = -angle;
					}
					transform.localRotation = Quaternion.Euler (0f, 0f, angle);
					float _distance = Vector3.Distance (Vector3.zero, new Vector3(endPos.x, endPos.y, 0));
					float scale = _distance * 1.12f / 0.6f;// / 0.6f +  0.15f / 0.6f;
					distance = scale;
				}
				passTime += Time.deltaTime;
				render.lengthScale = Mathf.Min (passTime / showTime, 1) * distance;
//				if(passTime >= showTime){
//					render = null;
//				}
			}
		}

		public void UpdateDist(float dis)
		{
			
			distance = dis;
		}

		private void RunTo (ParticleSystemRenderer render, float showTime, float distance)
		{
			this.render = render;
			this.showTime = showTime;
			this.distance = distance;
			this.render.lengthScale = 0;
			this.passTime = 0f;
		}

		private void setTarget (GameObject target)
		{
			this.target = target;
		}

		public static GameObject CreateRender (Transform parent, Vector3 startPos, Vector3 endPos, float showTime, GameObject go, GameObject target = null)
		{
			float distance = Vector3.Distance (Vector3.zero, new Vector3(endPos.x, endPos.y, 0));
			float angle = Vector2.Angle (Vector2.up, new Vector2 (endPos.x, endPos.y));
			if (endPos.x > 0) {
				angle = -angle;
			}
			go.transform.localPosition = startPos;
			go.transform.localRotation = Quaternion.Euler (0f, 0f, angle);
			TransformUtil.AddChild (parent, go.transform);

			LaserRay trail = go.GetComponent<LaserRay> ();
			if (trail == null) {
				trail = go.AddComponent<LaserRay> ();
			}
			float scale = distance * 1.12f / 0.6f;// / 0.6f +  0.15f / 0.6f;
			ParticleSystemRenderer render = go.GetComponentInChildren<ParticleSystemRenderer> ();
			trail.RunTo (render, showTime, scale);
			if (target != null) {
				trail.setTarget (target);
			}

			return trail.gameObject;
		}

	}

}