using UnityEngine;
using System.Collections;

public class ParticleHideObserver : MonoBehaviour {

	public float checkInterval = 1.0f;

	public delegate void EndEffect(GameObject partialObject);
	public event EndEffect endDelegate;

	public void ShowEffect() {
		ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
		for (int i=0; i<systems.Length; i++) {
			systems [i].time = 0;
			systems [i].Play();
		}
		gameObject.SetActive (true);
		InvokeRepeating("Check", checkInterval, checkInterval);
	}

	public void Check() {
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
			gameObject.SetActive (false);
			if (endDelegate != null) {
				endDelegate (this.gameObject);
			}
		}
	}
}