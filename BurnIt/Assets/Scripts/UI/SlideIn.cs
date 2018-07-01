using MTUnity.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlideDir
{
    LeftToRight,
    RightToLeft,
    UpToDown,
    DownToUp
}

[RequireComponent(typeof(RectTransform))]
public class SlideIn : MonoBehaviour {

    // Use this for initialization
    RectTransform rect;
    float height;
    float width;
    Vector3 originPos;

    public SlideDir _slideDir;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        height = rect.rect.height;
        width = rect.rect.width;

        switch(_slideDir)
        {
            case SlideDir.DownToUp:
                width = 0f;
                break;
            case SlideDir.UpToDown:
                height *= -1;
                width = 0f;
                break;
            case SlideDir.RightToLeft:
                height = 0;
                width *= -1;
                break;
            case SlideDir.LeftToRight:
                height = 0;
                
                break;
        }


        originPos = rect.anchoredPosition;


    }

    void OnEnable () {

       
        var newPos  = new Vector2(originPos.x + width, originPos.y + height);
        rect.anchoredPosition = originPos;
        gameObject.StopAllActions();
        gameObject.RunActions(new MTEaseOut(new MTUIAnchorPositionChangeTo(MoveTime, newPos),RATE));


	}

    private void OnDisable()
    {
        rect.anchoredPosition = originPos;
    }

    const float MoveTime = 0.2f;
    const float RATE = 0.6f;
    public float SlidOut()
    {
        gameObject.StopAllActions();
        gameObject.RunActions(new MTEaseOut(new MTUIAnchorPositionChangeTo(MoveTime, originPos), RATE));
        return MoveTime;
    }


}
