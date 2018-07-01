using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallSide
{
    Left,
    Right
}
public class Wall : MonoBehaviour {
    public WallSide _side;



    void Awake()
    {
        if(_side == WallSide.Left)
        {

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
            transform.position = worldPoint + Vector3.left * 0.5f;
        }
        else
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));
            transform.position = worldPoint + Vector3.right * 0.5f;
        }
    }


    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
