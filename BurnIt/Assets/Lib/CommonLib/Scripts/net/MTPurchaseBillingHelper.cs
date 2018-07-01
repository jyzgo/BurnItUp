using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MTUnity.Utils;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MTUnity {
	[System.Serializable]
	public enum PurchaseBillingChannelType {
		defalut, // android platform To google；ios platform To apple
		google,
		iOS
	}

	[System.Serializable]
	public class PurchaseBillingItem {
		public string iapKey;
		public string message;

		public PurchaseBillingItem(string key, string appId, string userId, string channel, Dictionary<string, object> infos) {
			iapKey = key;

			Dictionary<string, object> jObject = new Dictionary<string, object>(3 + infos.Count);
			jObject.Add ("appId", appId);
			jObject.Add ("userId", userId);
			jObject.Add ("channel", channel);
			foreach (string k in infos.Keys) {
				if (infos [k] == null) {
					jObject.Add (k, "");
				} else {
					jObject.Add (k, infos [k]);
				}
			}
			message = MTJSON.Serialize (jObject);
		}

		public PurchaseBillingItem(string key, string message_) {
			iapKey = key;
			message = message_;
		}

		public string DebugLog() {
			return string.Format ("[iapKey = {0}, message = {1}]", iapKey, message);
		}

		// other platform
		public static Dictionary<string, object> BuildGooglePlayBillingInfo(string signedData, string signature) {
			Dictionary<string, object> jObject = new Dictionary<string, object>(2);

			jObject.Add ("signedData", signedData);
			jObject.Add ("signature", signature);

			return jObject;
		}

		public static Dictionary<string, object> BuildAppleBillingInfo(string receipt, string transactionId) {
			Dictionary<string, object> jObject = new Dictionary<string, object>(1);

			jObject.Add ("receipt", receipt);
			jObject.Add ("transaction_id", transactionId);

			return jObject;
		}
	}

	[System.Serializable]
	public class PurchaseResult {
		public string iapKey;
		public int code;// 0 succ, 小于0 服务器返回错误码, 大于0 其他错误码

		public PurchaseResult(string iapKey_, int code_) {
			iapKey = iapKey_;
			code = code_;
		}
	}

	public class MTPurchaseBillingHelper : MonoBehaviour {
		public PurchaseBillingChannelType channelType; 
		public string appId;
		public string baseUrl = "http://track.magictavern.com/track.do";
		public string cacheFile = "/mtpb.bin";
		public string cacheKeyFile = "/mtpbk.bin";
		public string signKey;
		public float sendInterval = 0.2f;
		public float retryDelay = 5.0f;

		private List<PurchaseBillingItem> _queue;
		private bool _queueDirty;
		private bool _lastSucc = true;

		private List<PurchaseResult> _keys;
		private bool _keyDirty;

		protected virtual void OnCheckFinished(int code, string iapKey) {
		
		}
			
		/// <summary>
		/// 检测订单
		/// </summary>
		/// <param name="item">Item.</param>
		public void Check(PurchaseBillingItem item) {
			if (_queue == null) {
				LoadQueue ();
			}

			Debug.Log ("[MTPBHelper] Check item = " + item.DebugLog());

			_queue.Add (item);
			_queueDirty = true;
		}

		/// <summary>
		/// 获得第一个PurchaseResult
		/// </summary>
		/// <param name="remove">是否移除记录</param>
		public PurchaseResult GetFristPurchaseResult(bool remove) {
			PurchaseResult res = null;
			if (HasPurchaseResult()) {
				res = _keys [0];
				if (remove) {
					RemovePurchaseResult (res);
				}
			}

			return res;
		}

		/// <summary>
		/// 获得第一个PurchaseResult
		/// </summary>
		/// <param name="remove">是否移除记录</param>
		public PurchaseResult GetFristPurchaseResult(string key, bool remove) {
			PurchaseResult res = null;
			if (HasPurchaseResult()) {
				for(int i = 0; i < _keys.Count; i++) {
					if (_keys[i].iapKey.CompareTo(key) == 0) {
						res = _keys [i];
						if (remove) {
							RemovePurchaseResult (res);
						}
						break;
					}
				}
			}

			return res;
		}

		/// <summary>
		/// 获得第一个PurchaseResult
		/// </summary>
		/// <param name="remove">是否移除记录</param>
		public List<PurchaseResult> GetPurchaseResults(string key) {
			List<PurchaseResult> res = new List<PurchaseResult>();
			if (HasPurchaseResult()) {
				for(int i = 0; i < _keys.Count; i++) {
					if (_keys[i].iapKey.CompareTo(key) == 0) {
						res.Add (_keys [i]);
					}
				}
			}

			return res;
		}

		/// <summary>
		/// 获得第一个PurchaseResult
		/// </summary>
		/// <param name="remove">是否移除记录</param>
		public PurchaseResult GetFristPurchaseResult(string key, int code, bool remove) {
			PurchaseResult res = null;
			if (HasPurchaseResult()) {
				for(int i = 0; i < _keys.Count; i++) {
					if (_keys[i].iapKey.CompareTo(key) == 0 && _keys[i].code == code) {
						res = _keys [i];
						if (remove) {
							RemovePurchaseResult (res);
						}
						break;
					}
				}
			}

			return res;
		}

		/// <summary>
		/// 移除PurchaseResult
		/// </summary>
		public void RemovePurchaseResult(PurchaseResult result) {
			if (HasPurchaseResult()) {
				_keys.Remove (result);
				_keyDirty = true;
			}
		}

		/// <summary>
		/// 当前是否有PurchaseResult没有处理
		/// </summary>
		public bool HasPurchaseResult() {
			if (_keys == null) {
				LoadKeys ();
			}
			return _keys != null && _keys.Count > 0;
		}

		void Start() {
			if (channelType == PurchaseBillingChannelType.defalut) {
				if (Application.platform == RuntimePlatform.Android) {
					channelType = PurchaseBillingChannelType.google;
				} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
					channelType = PurchaseBillingChannelType.iOS;
				} else {
					Debug.LogWarning ("MTPurchaseBillingHelper  Awake  channelType = PurchaseBillingChannelType.defalut    platform = " + Application.platform.ToString ());
				}
			}

			if (_queue == null) {
				LoadQueue ();
			}
			if (_keys == null) {
				LoadKeys ();
			}

			StartCoroutine (sendLoop ());
		}

		void Update() {
			if (_queueDirty) {
				SaveQueue ();
			}
			if (_keyDirty) {
				SaveKeys ();
			}
		}

		void AddKey(PurchaseResult item) {
			if (_keys == null) {
				LoadKeys ();
			}
			if (_keys != null) {
				_keys.Add (item);
				_keyDirty = true;
			}
		}

		IEnumerator sendLoop() {
			while(true) {
				yield return new WaitForSeconds (_lastSucc ? sendInterval : retryDelay);

				if (_queue == null) {
					LoadQueue ();
				}

				if (_queue == null || _queue.Count == 0) {
					continue;
				}

				PurchaseBillingItem item = _queue [0];

				// sign
				string sign = MTSecurity.Md5Sum (item.message + signKey);
				Debug.Log ("[MTPBHelper] Sending check: " + item.message + "," + item.iapKey);
				Dictionary<string, string> headers = new Dictionary<string, string> ();
				headers ["sign"] = sign;
				headers ["Content-Type"] = "application/json";
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes (item.message);
				WWW www = new WWW (baseUrl, bytes, headers);
				yield return www;
				if (www.error != null) {
					Debug.Log ("[MTPBHelper] send Error: " + www.error);
					_lastSucc = false;
				} else { // success, remove from queue
					_queue.RemoveAt (0);
					_queueDirty = true;
					_lastSucc = true;

					int code = 1;
					if (www.text == null) {
						Debug.LogError ("[MTPBHelper] www.text = null");
					} else {
						MTJSONObject jObject = MTJSON.Deserialize (www.text);
						if (jObject == null) {
							Debug.LogError ("[MTPBHelper] JSON Object (www.text) = null");
							code = 2;
							AddKey (new PurchaseResult(item.iapKey, code));
						} else {
							jObject = jObject.Get ("ret_status");
							if (jObject == null) {
								Debug.LogError ("[MTPBHelper] ret_status = null, www.text = " + www.text);
								code = 3;
								AddKey (new PurchaseResult(item.iapKey, code));
							} else {
								Debug.Log ("[MTPBHelper] send Succ  code = " + code);
								code = jObject.i;
								AddKey (new PurchaseResult(item.iapKey, code));
							}
						}
					}
					OnCheckFinished (code, item.iapKey);
				}
			}
		}


		// file 
		// queue
		void LoadQueue() {
			if (_queue != null) {
				return;
			}
			_queueDirty = false;

			string filePath = Application.persistentDataPath + cacheFile;
			if (File.Exists (filePath)) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (filePath, FileMode.Open);
				if (file.Length == 0) {
					_queue = new List<PurchaseBillingItem> ();
					Debug.Log ("[MTPBHelper] queue loaded file.Length = 0 ");
				} else {
					_queue = (List<PurchaseBillingItem>)bf.Deserialize (file);
					Debug.Log ("[MTPBHelper] queue loaded with length of " + _queue.Count);
				}

				file.Close ();
			} else {
				_queue = new List<PurchaseBillingItem> ();
			}
		}

		void SaveQueue() {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create (Application.persistentDataPath + cacheFile); 
			bf.Serialize(file, _queue);
			file.Close();
			_queueDirty = false;
		}

		// keys
		void LoadKeys() {
			if (_keys != null) {
				return;
			}
			_keyDirty = false;

			string filePath = Application.persistentDataPath + cacheKeyFile;
			if (File.Exists (filePath)) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (filePath, FileMode.Open);

				if (file.Length == 0) {
					_keys = new List<PurchaseResult> ();
					Debug.Log ("[MTPBHelper] keys loaded file.Length = 0 ");
				} else {
					_keys = (List<PurchaseResult>)bf.Deserialize (file);
					Debug.Log ("[MTPBHelper] keys loaded with length of " + _keys.Count);
				}

				file.Close ();
			} else {
				_keys = new List<PurchaseResult> ();
			}
		}

		void SaveKeys() {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create (Application.persistentDataPath + cacheKeyFile); 
			bf.Serialize(file, _keys);
			file.Close();
			_keyDirty = false;
		}
	}

}
