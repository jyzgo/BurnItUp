using UnityEngine;
using System.Collections.Generic;
using MTUnity;

namespace MTUnity
{
	public class ModelPropChangedData
	{
		public string propKey;
		public object changeValue;
		public float delay = 0;

		public ModelPropChangedData ()
		{

		}

		public ModelPropChangedData (string propKey, object changeValue, float delay = 0)
		{
			this.propKey = propKey;
			this.changeValue = changeValue;
			this.delay = delay;
		}
	}

	public class ModelBinder : Singleton<ModelBinder>
	{
		public static void BindProp (string propKey, PropChangedListener l)
		{
			if (ModelBinder.Instance != null) {
				ModelBinder.Instance._BindProp (propKey, l);
			}
		}

		public static void UnbindProp (string propKey, PropChangedListener l)
		{
			if (ModelBinder.Instance != null) {
				ModelBinder.Instance._UnbindProp (propKey, l);
			}
		}

		public static void OnPropChanged (string propKey, object changeValue, float delay = 0)
		{
			if (ModelBinder.Instance != null) {
				ModelBinder.Instance._OnPropChanged (propKey, changeValue, delay);
			}
		}

		public static void OnPropChanged (ModelPropChangedData changedData)
		{
			if (ModelBinder.Instance != null) {
				ModelBinder.Instance._OnPropChanged (changedData);
			}
		}

		public delegate void PropChangedListener(object changeValue);

		Dictionary<string, PropChangedListener> _listeners;
		List<ModelPropChangedData> _delayedEvents;

		protected ModelBinder ()
		{
			_listeners = new Dictionary<string, PropChangedListener> ();
			_delayedEvents = new List<ModelPropChangedData> ();
		}

		void _BindProp (string propKey, PropChangedListener l)
		{
			if (_listeners.ContainsKey (propKey)) {
				_listeners [propKey] += l;
			} else {
				_listeners [propKey] = l;
			}
		}

		void _UnbindProp (string propKey, PropChangedListener l)
		{
			if (_listeners.ContainsKey (propKey)) {
				_listeners [propKey] -= l;
				if (_listeners [propKey] == null) {
					_listeners.Remove (propKey);
				}
			}
		}

		void _OnPropChanged (string propKey, object changeValue, float delay = 0)
		{
			if (delay > 0) {
				_delayedEvents.Add (new ModelPropChangedData (propKey, changeValue, delay));
				return;
			}

			InvokeListeners (propKey, changeValue);
		}

		void _OnPropChanged (ModelPropChangedData changedData)
		{
			if (changedData.delay > 0) {
				_delayedEvents.Add (changedData);
				return;
			}

			InvokeListeners (changedData.propKey, changedData.changeValue);
		}

		void InvokeListeners (string propKey, object changeValue)
		{
			if (_listeners.ContainsKey (propKey)) {
				_listeners [propKey].Invoke(changeValue);
			}
		}

		void Update ()
		{
			if (_delayedEvents.Count > 0) {
				for (int i = _delayedEvents.Count - 1; i >= 0; i--) {
					ModelPropChangedData data = _delayedEvents [i];
					data.delay -= Time.deltaTime;
					if (data.delay <= 0) {
						_delayedEvents.RemoveAt (i);
						InvokeListeners (data.propKey, data.changeValue);
					}
				}
			}
		}

	}
}
