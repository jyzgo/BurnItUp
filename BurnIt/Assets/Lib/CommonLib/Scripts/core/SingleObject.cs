// ------------------------------------------------------------------------------
// Author: billtt
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTUnity {

	/**
	 * Query and cache MonoBehaviour objects that solely exist in scene
	 * Usage:
	 * 		Hero hero = SingleObject.Get<Hero>(); // This will get an instance of the MonoBehaviour class Hero attached to a GameObject.
	 */
	public class SingleObject {
		private static Dictionary<Type, MonoBehaviour> _objects;

		public static T Get<T>() where T : MonoBehaviour {
			if (_objects == null) {
				_objects = new Dictionary<Type, MonoBehaviour>();
			}
			Type type = typeof(T);
			if (!_objects.ContainsKey(type) || _objects[type] == null) {
				T config = GameObject.FindObjectOfType<T>();
				_objects[type] = config;
			}
			return (T)_objects[type];
		}
	}
}

