using UnityEngine;
using System.Collections;

namespace MTUnity {

	/**
	 * Destroy the attached game object if all particle systems on it (including its children) have stopped
	 * Will destroy if no particle systems are found
	 */
	public class ParticleDestroyer : MonoBehaviour {

		public float checkInterval = 1.0f;
		
		// Use this for initialization
		void Start () {
			InvokeRepeating("Check", checkInterval, checkInterval);
		}
		
		void Check() {
			ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
			bool alive = false;
			for (int i=0; i<systems.Length; i++) {
				if (!systems[i].isStopped) {
					alive = true;
					break;
				}
			}
			if (!alive) {
				CancelInvoke();
				Destroy(gameObject);
			}
		}
	}

}

