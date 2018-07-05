using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMgr : MonoBehaviour {

    public static FireMgr Current;
    public GameObject FirePrefab;
    public GameObject FirePointPrefab;
    public GameObject Explosion;
    private void Awake()
    {
        Current = this;
    }



    internal void GenFirePoint(CommonBox commonBox,Vector2 pos)
    {
        var firePoint = Instantiate<GameObject>(FirePointPrefab);
        var firePointSc = firePoint.GetComponent<FirePoint>();
        firePoint.transform.SetParent(commonBox.transform);
        firePoint.transform.position = pos;
        var fire = Instantiate<GameObject>(FirePrefab);
        var fireSc = fire.GetComponent<Fire>();
        fireSc.SetFirePoint(firePointSc);
        
        firePointSc.AddFire(fire);
        firePointSc.AddBox(commonBox);
    }

}
