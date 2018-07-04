﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;
using System;

[RequireComponent(typeof(D2dDestructible))]
public class CommonBox : MonoBehaviour {
    private void Awake()
    {
        _des = GetComponent<D2dDestructible>();
        _destoryer = GetComponent<D2dDestroyer>();
    }

    D2dDestructible _des;
    D2dDestroyer _destoryer;

    public void Beburn(Vector2 pos)
    {
       
    }
    public void AddExpNum()
    {

        DesNum++;
        if(DesNum >= DESMAX)
        {
            _destoryer.enabled = true;
        }
    }

    public int DesNum = 0;
    public int DESMAX = 3;



    HashSet<Vector2> _firePos = new HashSet<Vector2>();
    internal void AddFire(Vector2 explosionPosition)
    {
         FireMgr.Current.GenFirePoint(this,explosionPosition);

       
    }
}
