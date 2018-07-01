using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdPlanB : AdState
{

    public override void OnDealAnimDone()
    {
        AdMgr.PreloadAdmobInterstitial();
    }

    public override void OnResetGame()
    {
        if (AdMgr.IsAdmobInterstitialReady())
        {

            AdMgr.ShowAdmobInterstitial();

        }
    }

    public override string AndroidInterstitialID()
    {
       // return "ca-app-pub-8151883983314364/3790306139";// magic
        return "ca-app-pub-7903543313139585/2137134754";
    }

    public override string IOSInterstitialID()
    {
        return "";
    }




}
