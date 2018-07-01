using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdState  {

    public abstract string AndroidInterstitialID();

    public abstract string IOSInterstitialID();

    public const string BannerID = "ca-app-pub-7903543313139585/9520800753";

    public virtual void OnDealAnimDone()
    {
        AdMgr.PreloadAdmobInterstitial();
    }

    public virtual void OnResetGame()
    {
    }

    public virtual void OnGameAnimDone()
    { }

}
