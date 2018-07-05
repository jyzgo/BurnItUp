using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;
using System;

[RequireComponent(typeof(D2dDestructible))]
public class CommonBox : MonoBehaviour {
    private void Awake()
    {
        _destructible = GetComponent<D2dDestructible>();
        _destoryer = GetComponent<D2dDestroyer>();
    }

    D2dDestructible _destructible;
    D2dDestroyer _destoryer;

    const int FractureCount = 2;
    internal void GetBurnt(FirePoint firePoint)
    {
        if (_destructible != null)
        {
            // Register split event
          //  _destructible.OnEndSplit.AddListener(OnEndSplit);

            // Split via fracture
            D2dQuadFracturer.Fracture(_destructible, FractureCount, 0.5f);

            // Unregister split event
            //_destructible.OnEndSplit.RemoveListener(OnEndSplit);
        }
            AddExpNum();
    }



    void AddExpNum()
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
