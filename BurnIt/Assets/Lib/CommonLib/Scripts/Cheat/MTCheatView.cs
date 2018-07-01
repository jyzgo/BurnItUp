using UnityEngine;
using System.Collections;

namespace MTUnity {

	public class MTCheatView : MonoBehaviour, MTCheatListener {
		public delegate void OnCoded(Vector3 pos, int code, bool finished);
		public event OnCoded myCoded;

		private float _lastCodeTime = 0;
		private string _code = "";
		private bool _authorized = true;

		void MTCheatListener.OnAuthorChanged (bool beforeAuthorized, bool newAuthorized) {
			_authorized = !newAuthorized;
		}

		void Awake() {
			MTCheat.Instance.AddListener (this);
		}

		// check code
		void Update () {
			if (!_authorized) {
				return;
			}
			bool check = false;
			Vector3 pos = Vector3.zero;
			if (Input.GetMouseButtonDown(0)) {
				pos = Input.mousePosition;
				check = true;
			}
			if (!check) {
				for (var i = 0; i < Input.touchCount; ++i) {
					Touch touch = Input.GetTouch(i);
					if (touch.phase == TouchPhase.Began) {
						pos = touch.position;
						check = true;
						break;
					}
				}
			}
			if (check) {
				float now = Time.time;
				if (now - _lastCodeTime > 3) { // reset
					_code = "";
				}
				_lastCodeTime = now;
				int code = (int)(pos.x * 3 / Screen.width) + 1;
				Debug.Log ("MTCheatView\tcode = " + code);
				_code = _code + code;

				bool finished = _code.Length == MTCheat.Instance.codeCount;
				if (myCoded != null) {
					myCoded.Invoke(Camera.main.ScreenToWorldPoint(pos), code, finished); 
				}

				if (finished) {
					MTCheat.Instance.Code = _code;
					_code = "";
				}
			}
		}
	}

}
