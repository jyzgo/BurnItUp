using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMgr : MonoBehaviour {

    public static FireMgr Current;
    public GameObject FirePrefab;
    public GameObject FirePointPrefab;
    private void Awake()
    {
        Current = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		
	}

    internal void AddFire(CommonBox commonBox)
    {
    }

    internal void GenFirePoint(CommonBox commonBox,Vector2 pos)
    {
        var firePoint = Instantiate<GameObject>(FirePointPrefab);
        var fireSc = firePoint.GetComponent<FirePoint>();
        firePoint.transform.SetParent(commonBox.transform);
        firePoint.transform.position = pos;
        var fire = Instantiate<GameObject>(FirePrefab);
        fireSc.AddFire(fire);
    }

}
