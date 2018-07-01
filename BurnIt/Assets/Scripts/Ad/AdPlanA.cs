using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdPlanA : AdState {



    public override void OnResetGame()
    {
        if (AdMgr.IsAdmobInterstitialReady())
        {
            if (!isShowed)
            {
                AdMgr.ShowAdmobInterstitial();
            }else
            {
                isShowed = false;
            }
        }
    }

    bool isShowed = false;

    public override void OnGameAnimDone()
    {
        if (AdMgr.IsAdmobInterstitialReady())
        {
            AdMgr.ShowAdmobInterstitial();
            isShowed = true;
        }
    }

    public override string AndroidInterstitialID()
    {
       // return "ca-app-pub-8151883983314364/3790306139"; //magic
        return "ca-app-pub-7903543313139585/9660401559";
    }

    public override string IOSInterstitialID()
    {
        return "";
    }

}
