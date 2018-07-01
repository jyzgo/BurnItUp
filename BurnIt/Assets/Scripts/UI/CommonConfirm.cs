using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommonConfirm : MonoBehaviour {

    public Canvas _curCanvas;

    public UnityEvent Yes;
    public UnityEvent No;

    public string content;
    public string title;

    public Text Content;
    public Text Title;

    public SlideIn _slide;
    private void Awake()
    {
        _curCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _curCanvas.worldCamera = Camera.main;

        SetContent(content);
        SetTitle(title);


    }

    public void SetContent(string str)
    {
        if (Content != null)
        {
            content = str;
            Content.text = content;
        }
    }


    public void SetTitle(string st)
    {
        if (Title != null)
        {
            title = st;
            Title.text = title;
        }
    }


    public void YesCall()
    {
        if (Yes != null)
        {
            Yes.Invoke();
        }
        var t = _slide.SlidOut();
        StartCoroutine(DisAble(t)); 


    }

    IEnumerator DisAble(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
    }

    public void Nocall()
    {
        if (No != null)
        {
            No.Invoke();
        }
        var t = _slide.SlidOut();
        StartCoroutine(DisAble(t));

    }

}
