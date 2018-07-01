using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using MTUnity.Utils;

namespace MTUnity {

	[Serializable]
	public class TrackItem {
		public int time;
		public string action;
		public string[] v;
		public int level;
		public int confVersion;
		public string appVersion;
		public bool persist = true;
	}
	
	public class MTTracker : MonoBehaviour {

		public static MTTracker Instance {
			get {
				return SingleObject.Get<MTTracker> ();
			}
		}

		public bool debug = false;

		public string appId;
		public string signKey;
		public string baseUrl = "http://track.magictavern.com/track.do";
		public string cacheFile = "/mtt.bin";
		public int maxBatch = 20;
		public float sendInterval = 0.2f;
		public float retryDelay = 5.0f;
		public float saveInterval = 10.0f;
		protected string _facebookId;
		protected int _confVersion;
		protected DeviceID _deviceID = null;

		private List<TrackItem> _queue;
		private bool _dirty;
		private double _lastSave;

		void Awake() {
			LoadQueue ();
		}

		void Start() {
			StartCoroutine (sendLoop ());
		}

		void Update() {
			double now = TimeUtil.GetUTCTimestamp ();
			if (_dirty && (now - _lastSave > saveInterval)) {
				SaveQueue ();
				_lastSave = now;
			}
		}

		public DeviceID TrackId {
			get {
				if (_deviceID == null) { 
					_deviceID = NativeUtil.GetDeviceIdentifier ();
					if (debug) Debug.Log ("Device ID is: " + _deviceID.id + " (" + _deviceID.type + ")");
					if (debug) Debug.Log ("Unity ID is: " + SystemInfo.deviceUniqueIdentifier);
				}
				return _deviceID;
			}
		}

		string GetClientType() {
			string client = null;
			if (Application.platform == RuntimePlatform.Android) {
				client = "Android";
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				client = "iOS";
			} else {
				client = Application.platform.ToString ();
			}
			return client;
		}

		protected void BuildMessage(List<TrackItem> items, out string jsonStr, out string sign)
		{
			Dictionary<string, object> json = BuildTrackMessage (items);

			if (json == null) {
				jsonStr = "";
				sign = "";
				return;
			}

			jsonStr = MTJSON.Serialize (json);
			// sign
			sign = MTSecurity.Md5Sum (jsonStr + signKey);
		}

		bool _lastSucc = true;
		IEnumerator sendLoop() {
			while (true) {
				yield return new WaitForSeconds (_lastSucc ? sendInterval : retryDelay);
				if (_queue.Count == 0) {
					continue;
				}
				// get list of items to send
				List<TrackItem> items = new List<TrackItem> ();
				items.Add (_queue[0]);
				for (int i=1; i<_queue.Count && items.Count < maxBatch; i++) {
					TrackItem item = _queue [i];
					if (item.appVersion != items[0].appVersion || item.confVersion != items[0].confVersion) { // batch items with the same app version and conf version
						continue;
					}
					items.Add (item);
				}

				string jsonStr;
				string sign;
				BuildMessage (items, out jsonStr, out sign);
				if (debug) Debug.Log ("Sending track: " + jsonStr);
				Dictionary<string, string> headers = new Dictionary<string, string> ();
				headers ["sign"] = sign;
				headers ["Content-Type"] = "application/json";
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes (jsonStr);
				WWW www = new WWW (baseUrl, bytes, headers);
				yield return www;
				if (!string.IsNullOrEmpty (www.error)) {
					if (debug) Debug.Log ("Error when sending track: " + www.error);
					_lastSucc = false;
					continue;
				} else { // success, remove from queue
					if (debug) Debug.Log ("Successfully sent track");
					for (int i=0; i<items.Count; i++) {
						if (items[i].persist) {
							_dirty = true;
						}
						_queue.Remove (items[i]);
					}
					_lastSucc = true;
				}
			}
		}

		void LoadQueue() {
			if (_queue != null) {
				return;
			}

			_dirty = false;

			string filePath = Application.persistentDataPath + cacheFile;
			if (File.Exists (filePath)) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (filePath, FileMode.Open);
				try
				{            
					_queue = (List<TrackItem>)bf.Deserialize (file);
				}
				catch
				{
					// deserialize failed
					_queue = new List<TrackItem> ();
				}

				file.Close ();
				if (debug) Debug.Log ("Track queue loaded with length of " + _queue.Count);
			} else {
				_queue = new List<TrackItem> ();
			}
		}

		public void SaveQueue() {
			// get items that need persisting
			List<TrackItem> items = new List<TrackItem> ();
			for (int i=0; i<_queue.Count; i++) {
				TrackItem item = _queue [i];
				if (item.persist) {
					items.Add (item);
				}
			}

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create (Application.persistentDataPath + cacheFile); 
			bf.Serialize(file, items);
			file.Close();
			_dirty = false;
		}

		public void UpdateUser(string facebookId) {
			_facebookId = facebookId;
		}

		public void UpdateConfVersion(int version) {
			_confVersion = version;
		}

		/**
		 * Track with persist=true
		 */
		public void Track(string action, int level, params string[] v) {
			Track (action, level, true, v);
		}

		public void Track(string action, int level, bool persist, params string[] v) {
			if (_queue == null) {
				LoadQueue ();
			}
			_queue.Add (BuildTrackItem(action, level, persist, v));
			if (persist) {
				_dirty = true;
			}
		}

		public virtual void TrackInstall() {
			string v1 = SystemInfo.deviceModel;
			string v2 = SystemInfo.operatingSystem;
            string v3 = string.Format("{0}", (int)TrackId.type);
            string v4 = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString();
            Track ("install", 0, v1, v2, v3, v4);
		}

		/// <summary>
		/// 设备信息。
		/// </summary>
		public void TrackDeviceInfo ()
		{
			string v1 = SystemInfo.deviceModel;
			string v2 = SystemInfo.operatingSystem;
			string v3 = SystemInfo.systemMemorySize.ToString ();
			string v4 = SystemInfo.graphicsDeviceName;
			string v5 = SystemInfo.graphicsMemorySize.ToString ();
			string v6 = SystemInfo.graphicsDeviceType.ToString ();

			string v7 = "";
			var itemEnum = System.Enum.GetValues (typeof (TextureFormat)).GetEnumerator ();
			int lastFormatValue = 0;
			int n = 0;
			while (itemEnum.MoveNext ()) {
				TextureFormat format = (TextureFormat)itemEnum.Current;
				bool isSupport = false;
				try {
					isSupport = SystemInfo.SupportsTextureFormat (format);
				} catch (System.Exception) {
					isSupport = false;
				}
				if (isSupport) {
					int formatValue = (int)format;
					if (n == 0) {
						v7 += formatValue.ToString ("X");
						n = 1;
					} else if (formatValue == lastFormatValue + n) {
						n++;
					} else {
						if (n > 1) v7 += "-" + n.ToString ("X");
						v7 += "|" + formatValue.ToString ("X");

						lastFormatValue = formatValue;
						n = 1;
					}
				}
			}
			if (n > 1) v7 += "-" + n.ToString ("X");

			string v8 = string.Format ("{0}|{1}", 
				SystemInfo.maxTextureSize,
				SystemInfo.graphicsShaderLevel);
			Track ("device_info", 0, v1, v2, v3, v4, v5, v6, v7, v8);
		}

		public TrackItem BuildTrackItem(string action, int level, bool persist, params string[] v) {
			TrackItem item = new TrackItem ();

			item.action = action;
			item.v = v;
			item.appVersion = Application.version;
			item.confVersion = _confVersion;
			item.time = (int)TimeUtil.GetUTCTimestamp ();
			item.level = level;
			item.persist = persist;

			return item;
		}

		public Dictionary<string, object> BuildTrackMessage(List<TrackItem> items) {
			if (items == null || items.Count == 0) {
				return null;
			}

			Dictionary<string, object> json = new Dictionary<string, object>(7);

			// common parameters
			json.Add ("app", appId);
			json.Add ("client", GetClientType());
			json.Add ("uid", TrackId.id);
			json.Add ("confVersion", items[0].confVersion);
			json.Add ("ver", items[0].appVersion);
			json.Add ("fbid", _facebookId);

			// add events
			List<object> events = new List<object>(items.Count);
			for (int i = 0; i < items.Count; i++) {
				TrackItem item = items [i];
				Dictionary<string, object> jItem = new Dictionary<string, object>(3 + item.v.Length);
				jItem.Add ("action", item.action);
				jItem.Add ("level", item.level);
				jItem.Add ("time", "" + item.time);
				for (int j = 0; j < item.v.Length; j++) {
					string v = item.v [j];
					if (v == null || v.Length == 0) {
						continue;
					}
					jItem.Add ("v" + (j + 1), v);
				}
				events.Add (jItem);
			}
			json.Add ("events", events);

			return json;
		}
	}
}

