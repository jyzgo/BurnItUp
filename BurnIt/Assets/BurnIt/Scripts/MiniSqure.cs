using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSqure : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    SpriteRenderer _render;
    private void Awake()
    {
        _render = GetComponent<SpriteRenderer>();
    }

    int life = 5;


    private void OnMouseUpAsButton()
    {
        life--;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            float reduce = (5 - life) * 0.1f;
            Debug.Log("press");
            _render.color = new Color(1 - reduce, 1 - reduce, 1 - reduce);
        }
    }
}
