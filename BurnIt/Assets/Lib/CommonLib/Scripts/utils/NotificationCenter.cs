using UnityEngine;
using System.Collections;
using MTUnity;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MTUnity{


	public class NotificationCenter : Singleton<NotificationCenter> {
		
		private Dictionary <string, UnityEvent> _eventDictionary;

		void Awake ()
		{
			if (_eventDictionary == null)
			{
				_eventDictionary = new Dictionary<string, UnityEvent>();
			}
		}
		
		public static void AddListener (string eventName, UnityAction listener)
		{
			UnityEvent thisEvent = null;
			if (Instance._eventDictionary.TryGetValue (eventName, out thisEvent))
			{
				thisEvent.AddListener (listener);
			} 
			else
			{
				thisEvent = new UnityEvent ();
				thisEvent.AddListener (listener);
				Instance._eventDictionary.Add (eventName, thisEvent);
			}
		}
		
		public static void RemoveListener (string eventName, UnityAction listener)
		{
			if (Instance == null) return;
			UnityEvent thisEvent = null;
			if (Instance._eventDictionary.TryGetValue (eventName, out thisEvent))
			{
				thisEvent.RemoveListener (listener);
			}
		}
		
		public static void Post (string eventName,params ObserverInfo[]x)
		{
			UnityEvent thisEvent = null;
			if (Instance._eventDictionary.TryGetValue (eventName, out thisEvent))
			{
				thisEvent.Invoke ();
			}
		}
	}




//	public enum ObserverType{
//		PropNumChanged,
//		MovesChanged
//	}
//
	public class ObserverInfo //inherit this are able to  pass any object to any Listener
	{
		public string str;
		public int n;
		public float f;
	}
//
//	public delegate void ObserverCallBack(ObserverType curType,params ObserverInfo []x);
//
//	public interface IListener
//	{
//		void OnNotification(ObserverType curType,params ObserverInfo [] x);
//	}
//	
//
//	public class NotificationCenter : Singleton<NotificationCenter> {
//		Dictionary<ObserverType,ObserverCallBack> _listenerDict = new Dictionary<ObserverType,ObserverCallBack>();
//		Dictionary<IListener,List<ObserverType>> _listerHoldType = new Dictionary<IListener, List<ObserverType>>();
//
//		public void Add(ObserverType curType,IListener curListener)
//		{
//			Debug.Log ("Add listener " + curType.ToString());
//			if(_listenerDict.ContainsKey(curType))
//			{
//				Debug.Log("plus ");
//				_listenerDict[curType] += curListener.OnNotification;
//			}else
//			{
//				_listenerDict[curType] = curListener.OnNotification;
//				_listerHoldType[curListener] = new List<ObserverType>();
//				_listerHoldType[curListener].Add(curType);
//			}
//		}
//
//		public void Remove(IListener curListener)
//		{
//
//			List<ObserverType> curTypeList = null;
//
//			if(_listerHoldType.TryGetValue(curListener,out curTypeList))
//			{
//				for(int i = 0 ; i < curTypeList.Count;i ++)
//				{
//					var curType = curTypeList[i];
//					ObserverCallBack curCallBack = null;
//					if(_listenerDict.TryGetValue(curType,out curCallBack))
//					{
//						Debug.Log ("Remove listener " + curType.ToString());
//						curCallBack -= curListener.OnNotification;
//					}
//
//				}
//				_listerHoldType.Remove(curListener);
//			}
//
////
////			if(_listerHoldType.ContainsKey(curListener))
////			{
////				var curTypeList = _listerHoldType[curListener];
////				if(curTypeList != null)
////				{
////					for(int i  = 0 ;  i < curTypeList.Count ; i ++)
////					{
////						var curType = curTypeList[i];
////						if(_listenerDict.ContainsKey(curType))
////						{
////							Debug.Log ("Remove listener " + curType.ToString());
////							_listenerDict[curType] -= curListener.OnNotification;
////						}
////					}
////				}
////				_listerHoldType.Remove(curListener);
////			}
//
//		}
//
//		public static void AddListener(ObserverType curType,IListener curListener)
//		{
//			var curInstance = NotificationCenter.Instance;
//			if(curInstance != null)
//				curInstance.Add(curType,curListener);
//		}
//
//		public static void RemoveListener(IListener curListener)
//		{
//			var curInstance = NotificationCenter.Instance;
//			if(curInstance != null)
//				curInstance.Remove(curListener);
//		}
//
//		public static void Post(ObserverType curType,params ObserverInfo[]x)
//		{
//			var curInstance = NotificationCenter.Instance;
//			if(curInstance != null)
//				curInstance.PostListener(curType,x);
//		}
//
//		public void PostListener(ObserverType curType,params ObserverInfo[]x)
//		{
//			if(_listenerDict.ContainsKey(curType))
//			{
//				_listenerDict[curType](curType,x);
//			}
//		}
//	}
}