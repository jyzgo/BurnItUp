using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
		if(_fire!= null)
        {
            _fire.transform.position = transform.position;
          
        }
	}

    GameObject _fire = null;
    internal void AddFire(GameObject fire)
    {
        _fire = fire;
    }
}
