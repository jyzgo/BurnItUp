using UnityEngine;

namespace MTUnity.Utils
{
	public class TransformUtil
	{

		public static void AddChild (Transform parent, Transform child, bool worldPositionStays = false)
		{
			child.SetParent (parent, worldPositionStays);
		}

		public static void SwapPosition (Transform t1, Transform t2, bool local = true)
		{
			Vector3 pos1 = local ? t1.localPosition : t1.position;
			if (local) {
				t1.localPosition = t2.localPosition;
				t2.localPosition = pos1;
			} else {
				t1.position = t2.position;
				t2.position = pos1;
			}
		}

		public static void SwapPositionXY (Transform t1, Transform t2, bool local = true)
		{
			Vector3 pos1 = local ? t1.localPosition : t1.position;
			Vector3 pos2 = local ? t2.localPosition : t2.position;
			float z1 = pos1.z;
			pos1.z = pos2.z;
			pos2.z = z1;
			if (local) {
				t1.localPosition = pos2;
				t2.localPosition = pos1;
			} else {
				t1.position = pos2;
				t2.position = pos1;
			}
		}

		public static void SetPositionX (Transform t, float newX, bool local = true)
		{
			Vector3 pos = local ? t.localPosition : t.position;
			pos.x = newX;
			if (local)
				t.localPosition = pos;
			else
				t.position = pos;
		}

		public static void SetPositionY (Transform t, float newY, bool local = true)
		{
			Vector3 pos = local ? t.localPosition : t.position;
			pos.y = newY;
			if (local)
				t.localPosition = pos;
			else
				t.position = pos;
		}

		public static void SetPositionZ (Transform t, float newZ, bool local = true)
		{
			Vector3 pos = local ? t.localPosition : t.position;
			pos.z = newZ;
			if (local)
				t.localPosition = pos;
			else
				t.position = pos;
		}

		public static void SetPositionXY (Transform t, float newX, float newY, bool local = true)
		{
			Vector3 pos = local ? t.localPosition : t.position;
			pos.x = newX;
			pos.y = newY;
			if (local)
				t.localPosition = pos;
			else
				t.position = pos;
		}

		public static void AddPositionX (Transform t, float deltaX, bool local = true)
		{
			Vector3 pos = local ? t.localPosition : t.position;
			pos.x += deltaX;
			if (local)
				t.localPosition = pos;
			else
				t.position = pos;
		}

		public static void AddPositionY (Transform t, float deltaY, bool local = true)
		{
			Vector3 pos = local ? t.localPosition : t.position;
			pos.y += deltaY;
			if (local)
				t.localPosition = pos;
			else
				t.position = pos;
		}

		public static void AddPositionZ (Transform t, float deltaZ, bool local = true)
		{
			Vector3 pos = local ? t.localPosition : t.position;
			pos.z += deltaZ;
			if (local)
				t.localPosition = pos;
			else
				t.position = pos;
		}

		public static void AddPositionXY (Transform t, float deltaX, float deltaY, bool local = true)
		{
			Vector3 pos = local ? t.localPosition : t.position;
			pos.x += deltaX;
			pos.y += deltaY;
			if (local)
				t.localPosition = pos;
			else
				t.position = pos;
		}

		/// <summary>
		/// Finds the sub child.
		/// 递归查找制定name的子对象
		/// </summary>
		/// <returns>The sub child.</returns>
		/// <param name="parent">Parent.</param>
		/// <param name="childName">Child name.</param>
		public static GameObject FindSubChild (GameObject parent, string childName)
		{
			if (parent) {
				int childCount = parent.transform.childCount;
				for (int i = 0; i < childCount; i++) {
					Transform tf = parent.transform.GetChild (i);
					if (tf.gameObject.name == childName) {
						return tf.gameObject;
					}
				}
				for (int i = 0; i < childCount; i++) {
					Transform tf = parent.transform.GetChild (i);
					GameObject subTf = FindSubChild (tf.gameObject, childName);
					if (subTf) {
						return subTf;
					}
				}
			}
			return null;
		}

		public static GameObject FindSubChild (Transform parent, string childName)
		{
			if (parent) {
				return FindSubChild (parent.gameObject, childName);
			}
			return null;
		}

		public static void RemoveAllChildren(Transform parent) {
			if (parent) {
				for(int i = parent.childCount - 1; i >= 0; i--) {
					GameObject.Destroy (parent.GetChild(i).gameObject);
				}
			}
		}
	}

}