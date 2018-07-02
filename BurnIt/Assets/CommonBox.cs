using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;
[RequireComponent(typeof(D2dDestructible))]
public class CommonBox : MonoBehaviour {
    private void Awake()
    {
        _des = GetComponent<D2dDestructible>();
    }

    D2dDestructible _des;

    public void Beburn(Vector2 pos)
    {
       
    }

    public void AddExpNum()
    {
        Debug.Log("111");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
