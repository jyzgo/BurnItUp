using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    private void Awake()
    {
        
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3( Screen.width/2,0, 0));
        transform.position = worldPoint + Vector3.down*0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
