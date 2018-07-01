using UnityEngine;
using UnityEngine.EventSystems;

namespace MTUnity.Utils
{
	
	public class EventUtil
	{

		/// <summary>
		/// EventSystem是否接收到了鼠标事件，例如在UIElement上点击，则返回值为true
		/// </summary>
		public static bool IsPointerOverEventSystem ()
		{
			return EventSystem.current.IsPointerOverGameObject ();
		}

		/// <summary>
		/// 检测2D环境中的鼠标事件是否由指定的对象所触发
		/// </summary>
		public static bool CheckMouseHit2D (GameObject go)
		{
			Vector2 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (point, Vector2.zero);
			if (hit.collider != null && hit.collider.gameObject == go) {
				return true;
			}

			return false;
		}

		
		
	}
	
}