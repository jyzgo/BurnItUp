using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

namespace MTUnity {
	public interface MTCheatListener {
		void OnAuthorChanged (bool beforeAuthorized, bool newAuthorized);
	}

	public class MTCheat : MonoBehaviour {
		public string baseUrl = "http://ct.magictavern.com/cheat";
		public string appId = "app1001";

		public string remoteCheckBase = "https://graph.facebook.com";

		public string localFileName = "/mtauth.bin";

		public int codeCount = 10;

		private static int remoteCheckBaseCode = 3034421;
		private bool _dirty = false;

		private string _userId = "";
		private string _code = "";

		public float UpdateInterval = 0.5F;
		private float _fLastInterval;
		private int _iFrames = 0;
		private float _fFps;

		private bool _authorized = false;
		private List<MTCheatListener> _listeners = new List<MTCheatListener> ();

		private static MTCheat _instance = null;
		public static MTCheat Instance {
			get {
				return _instance;
			}
		}

		public string UserId {
			set { 
				_userId = value;
				_dirty = true;
			}
		}

		public string Code {
			set { 
				_code = value;
				_dirty = true;
			}
		}

		public bool Authorized {
			get { 
				return _authorized;
			}
		}

		public void TryGetAuth() {
			_dirty = false;
			StartCoroutine (CheckAuthByHttp());
		}

		public void TryLocalGetAuth() {
			ChangeAuthorized (CheckAuthor (GetLocalAuthor()));
		}

		public void AddListener(MTCheatListener listener) {
			for(int i = 0; i < _listeners.Count; i++) {
				if (listener == _listeners[i]) {
					return;
				}
			}

			_listeners.Add (listener);
		}

		public bool DeleteLocalDataFile() {
			string fn = Application.persistentDataPath + localFileName;

			if (File.Exists (fn)) {
				Debug.Log ("DeleteLocalDataFile:"+fn);
				File.Delete (fn);
				ChangeAuthorized (false);
				return true;
			}

			return false;
		}

		void Awake()
		{
			if(Instance != null && Instance != this)
			{
				Destroy(this.gameObject);
				return;
			}
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}

		void Start() {
			TryLocalGetAuth ();
			_fLastInterval = Time.realtimeSinceStartup;
			_iFrames = 0;
		}

		void Update() {
			if (_dirty && _userId.Length > 0 && _code.Length > 0) {
				TryGetAuth ();
			}
			_iFrames++;
			if (Time.realtimeSinceStartup > _fLastInterval + UpdateInterval)
			{
				_fFps = _iFrames / (Time.realtimeSinceStartup - _fLastInterval);
				_iFrames = 0;
				_fLastInterval = Time.realtimeSinceStartup;
			}
			if (_authorized == true)
			{
				GameObject btnCheat = GameObject.Find("BtnCheat");
				if (btnCheat != null)
				{
					Text text = btnCheat.transform.GetComponentInChildren<Text>();
					if(text != null)
						text.text = "FPS: "+_fFps.ToString("0.00");
				}
			}
		}

		IEnumerator CheckAuthByHttp()
		{
			WWW www = new WWW (GetUrl());
			yield return www;

			if(string.IsNullOrEmpty(www.error)) {
				if (CheckAuthor (www.text)) {
					SaveAuthorToLocal (www.text);
					ChangeAuthorized (true);
				} else {
					ChangeAuthorized (false);
				}
			} else {
				Debug.Log ("MTCheat\tCheckAuthByHttp" + www.error);
			}
		}

		private string GetUrl() {
			string res = baseUrl;
			if (appId.Length > 0) {
				if (res.Length == baseUrl.Length) {
					res = res + "?appId=" + appId;
				} else {
					res = res + "&appId=" + appId;
				}
			}
			if (_userId.Length > 0) {
				if (res.Length == baseUrl.Length) {
					res = res + "?id=" + _userId;
				} else {
					res = res + "&id=" + _userId;
				}
			}
			if (_code.Length > 0) {
				if (res.Length == baseUrl.Length) {
					res = res + "?code=" + _code;
				} else {
					res = res + "&code=" + _code;
				}
			}

			Debug.Log ("MTCheat\tGetUrl" + res);
			return res;
		}

		private bool CheckAuthor(string res) {
			if (res.Length > 0) {
				string md5 = MTSecurity.Md5Sum(GetAuthorBase());
				Debug.Log ("MTCheat\tCheckAuthor\tres = " + res + ", md5 = " + md5);
				if (res.CompareTo(md5) == 0) {
					return true;
				}
			}

			return false;
		}

		private string GetAuthorBase() {
			string res = string.Format ("{0}/{1}", remoteCheckBase, remoteCheckBaseCode);
			Debug.Log ("MTCheat\tGetAuthorBase\t1\tres = " + res);
			if (appId.Length > 0 && _userId.Length > 0) {
				res = res + "/" + appId + _userId;
			}
			Debug.Log ("MTCheat\tGetAuthorBase\t2\tres = " + res);
			return res;
		}

		private void SaveAuthorToLocal(string author) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create (Application.persistentDataPath + localFileName); 
			bf.Serialize(file, author);
			file.Close();
		}

		private string GetLocalAuthor() {
			string res = "";
			string filePath = Application.persistentDataPath + localFileName;
			if (File.Exists (filePath)) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (filePath, FileMode.Open);
				res = (string)bf.Deserialize (file);
				file.Close ();
			} 

			return res;
		}

		private void ChangeAuthorized(bool authorized) {
			Debug.Log ("MTCheat\tChangeAuthorized\t" + authorized);
			#if UNITY_EDITOR
			authorized = true;
			Debug.LogWarning ("But It's in Editor so ChangeAuthorized\t" + authorized);
			#endif

			for(int i = _listeners.Count - 1; i >= 0; i--) {
				if (_listeners [i] == null) {
					_listeners.RemoveAt (i);
				} else {
					_listeners [i].OnAuthorChanged (_authorized, authorized);
				}
			}
			_authorized = authorized;
		}
	}

}
