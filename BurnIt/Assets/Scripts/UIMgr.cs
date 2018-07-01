using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour {

    
    public Text _stateText;
    private void Awake()
    {
        

    }

    internal void SetStateText(string v)
    {
        _stateText.text = v;
    }
}
