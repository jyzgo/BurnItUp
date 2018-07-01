using UnityEngine;
using System.Collections;
using MTUnity.Actions;

public class ScaleAnim : MonoBehaviour {

    void Awake()
    {
        _oriScale = transform.localScale;
    }

    Vector3 _oriScale;

    const float t = 0.1f;
    void OnEnable()
    {
        transform.localScale = new Vector3(t, 0.1f, 0.1f);
        var scale1 = new MTScaleTo(t * 2f, _oriScale * 1.1f);
//        var scale2 = new MTScaleTo(t, 0.95f * _oriScale);
//        var scale3 = new MTScaleTo(t, 1.02f * _oriScale);
        var scale4 = new MTScaleTo(t, _oriScale);
        this.StopAllActions();
		this.RunAction(new MTEaseInOut(new MTSequence(scale1, scale4),0.5f));


    }

}
