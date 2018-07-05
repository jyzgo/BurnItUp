using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour {


	
	// Update is called once per frame


    GameObject _fire = null;
    internal void AddFire(GameObject fire)
    {
        _fire = fire;
    }

    CommonBox _commonBox;
    internal void AddBox(CommonBox commonBox)
    {
        _commonBox = commonBox;
    }

    public CommonBox GetBox()
    {
        return _commonBox;
    }

    private void OnDestroy()
    {
        if(_fire != null)
        {
            Destroy(_fire, 2f);
            _fire.SetActive(false);
        }
    }
}
