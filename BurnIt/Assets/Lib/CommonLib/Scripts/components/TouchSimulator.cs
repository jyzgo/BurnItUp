using UnityEngine;
using System;

namespace MTUnity
{
	public class TouchSimulator : MonoBehaviour
	{
		private float _longPressTime = 0;
		private float _longPressMeasure = 0.6f;
		private Action<Vector2> _onTouchBegan;
		private Action<Vector2> _onTouchMoved;
		private Action<Vector2> _onTouchEnded;
		private Action<Vector2> _onLongPressed;
		private bool _beginLongPressed = false;

		private TouchPhase _phase = TouchPhase.Canceled;
		public TouchPhase phase {
			get{ return _phase; }
		}

		private Vector2 _lastMousePosition;

		/// <summary>
		/// 注册触摸事件处理器
		/// </summary>
		public void RegisterEventHandler (Action<Vector2> onTouchBegan, Action<Vector2> onTouchMoved, Action<Vector2> onTouchEnded, Action<Vector2> onLongPressed )
		{
			_onTouchBegan = onTouchBegan;
			_onTouchMoved = onTouchMoved;
			_onTouchEnded = onTouchEnded;
			_onLongPressed = onLongPressed;
		}

		public void CancelTouch ()
		{
			_phase = TouchPhase.Canceled;
		}

		void OnEnable ()
		{
			Collider2D collider = gameObject.GetComponent<Collider2D> ();
			if (collider == null) {
				Debug.LogWarning ("TouchSimulator:: No collider 2d component.");
			}
		}

		void OnDisable ()
		{
			CancelTouch ();
		}

		void Update ()
		{
			int state = 0;
			if (Input.GetMouseButtonDown (0)) {
				state = 1;
			} else if (Input.GetMouseButton (0)) {
				state = 2;
			} else if (Input.GetMouseButtonUp (0)) {
				state = 3;
			}
				
			if (state != 0) {
				if ((state == 2 || state == 3) && _phase == TouchPhase.Canceled) {
					return;
				}

				Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast ((Vector2)point, Vector2.zero);
				if (hit.collider != null && hit.collider.gameObject == gameObject) {
					Vector2 pos = transform.InverseTransformPoint (point);

					TouchPhase newPhase = _phase;
					if (state == 1) {
						newPhase = TouchPhase.Began;
					} else if (state == 2) {
						if (pos == _lastMousePosition) {
							newPhase = TouchPhase.Stationary;
						} else {
							newPhase = TouchPhase.Moved;
						}
					} else if (state == 3) {
						newPhase = TouchPhase.Ended;
					}
					if (newPhase != _phase || newPhase == TouchPhase.Moved) { 
						_phase = newPhase;
						_lastMousePosition = pos;
						TouchHandler ();
					}
				}
			}

			if (_phase == TouchPhase.Stationary)
			{
				if (_beginLongPressed)
				{
					float time = Time.time;
					if (time - _longPressTime >= _longPressMeasure && _onLongPressed != null)
					{
						_onLongPressed(_lastMousePosition);
						_beginLongPressed = false;
					}
				}
			}
			else
			{
				_beginLongPressed = false;
			}
		}

		void OnMouseExit ()
		{
			if (_phase == TouchPhase.Canceled)
				return;

			if (_phase != TouchPhase.Ended) {
				//在鼠标按下拖拽状态下直接移出Collider时，强制结束触摸
				Debug.Log ("TouchSimulator::OnMouseExit: Force end touching.");
				if (_onTouchEnded != null)
					_onTouchEnded (_lastMousePosition);
			}
			CancelTouch ();
		}

		void TouchHandler ()
		{
//			Debug.Log ("TouchSimulator::TouchHandler: phase=" + _phase.ToString ());

			switch (_phase) {
			case TouchPhase.Began:
				if (_onTouchBegan != null)
					_onTouchBegan (_lastMousePosition);
				break;
			case TouchPhase.Moved:
				if (_onTouchMoved != null)
					_onTouchMoved (_lastMousePosition);
				break;
				case TouchPhase.Stationary:
					{
						_beginLongPressed = true;
						_longPressTime = Time.time;
					}
				break;
			case TouchPhase.Ended:
				if (_onTouchEnded != null)
					_onTouchEnded (_lastMousePosition);
				break;
			}
		}


	}
}

