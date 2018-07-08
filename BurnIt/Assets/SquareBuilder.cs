using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBuilder : MonoBehaviour {

    public int Size = 3;
    float m = 0.32f;
    // Use this for initialization
    Rigidbody2D dd = null;
	void Start () {
        List<List<GameObject>> _list = new List<List<GameObject>>();
        for(int i = 0; i <Size; i ++)
        {
            List<GameObject> inner = new List<GameObject>();
            _list.Add(inner);
            for(int j = 0; j< Size; j++)
            {
                var gb = Instantiate<GameObject>(ResMgr.Current.SmallSquare);
                inner.Add(gb);
                gb.name = "r" + i + "c" + j;
                gb.transform.SetParent(transform);
                gb.transform.position = new Vector2(i * m, j * m);
                FixedJoint2D f = gb.GetComponent<FixedJoint2D>();
                if(dd != null)
                {
                   f.connectedBody= dd;
                }
                dd = gb.GetComponent<Rigidbody2D>();
                if(i > 0)
                {
                    var s = _list[i - 1];
                    var sBody =  s[j].GetComponent<Rigidbody2D>();
                    var f2 = gb.AddComponent<FixedJoint2D>();
                    f2.connectedBody = sBody;

                }
            }
        }

        Rigidbody2D reversBody = null;
        for(int i = Size-1;i >=0;i--)
        {
            for (int j = Size -1;j>=0; j--)
            {
                var gb = _list[i][j];
        
      
            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
