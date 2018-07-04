using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    ParticleSystem _sys;
    private void Awake()
    {
        _sys = GetComponent<ParticleSystem>();
    }
    // Use this for initialization
    void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
