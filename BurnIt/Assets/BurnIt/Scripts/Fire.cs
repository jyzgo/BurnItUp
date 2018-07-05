using System;
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


    void Update()
    {

        transform.position = _firePoint.transform.position + Vector3.up * 0.8f;
        if (Time.time > burstTime + FIRE_INTERVAL)
        {
            burstTime = Time.time;
            var box = _firePoint.GetBox();
            if (box != null)
            {
                box.GetBurnt(_firePoint);
            }
        }
 



    }

    const float FIRE_INTERVAL = 1f;
    float burstTime = 0f;

    FirePoint _firePoint;
    internal void SetFirePoint(FirePoint firePoint)
    {
        _firePoint = firePoint;
    }
}
