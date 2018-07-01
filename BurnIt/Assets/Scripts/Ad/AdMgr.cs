

using GoogleMobileAds.Api;
using MTUnity;
using System;
using UnityEngine;

public static class AdMgr  {
    #region Admob-------------------------
//#if (UNITY_IOS)
//    const string BANNER_ID = "ca-app-pub-8151883983314364/7268920130";
    
//    const string INTERSTITIAL_ID = "ca-app-pub-8151883983314364/4175852934";
////#elif (UNITY_ANDROID)

//    const string MAGIC_NATIVE_BANNER_ID = "ca-app-pub-8151883983314364/2313572938";
//    const string MAGIC_INTERSTITIAL_ID = "ca-app-pub-8151883983314364/3790306139";
//    const string MAGIC_BANNER = "ca-app-pub-8151883983314364/7016837337";
    //#endif


    const string MAGIC_IOS_INTERSTITIAL_ID = "ca-app-pub-8151883983314364/4175852934";
    const string MAGIC_IOS_BANNER_ID = "ca-app-pub-8151883983314364/7268920130";


    const string OMG_IOS_INSTERSTITIAL_ID = "ca-app-pub-9169799985632280/8700464051";
    const string OMG_IOS_BANNER_ID = "ca-app-pub-9169799985632280/4013654050";


    //#if (UNITY_IOS)
    //    const string OMG_NATIVE_BANNER_ID = "ca-app-pub-9169799985632280/7223730851";
    //    const string OMG_INTERSTITIAL_ID = "ca-app-pub-9169799985632280/8700464051";
    //#elif (UNITY_ANDROID)
    //    const string OMG_NATIVE_BANNER_ID = "ca-app-pub-9169799985632280/9572081659";
    //    const string OMG_INTERSTITIAL_ID = "ca-app-pub-9169799985632280/2048814858";
    //#endif



    public static void ShowAdmobInterstitial()
    {
        _interstitial.Show();
    }

    public static void PreloadAdmobInterstitial()
    {

#if UNITY_ANDROID
        string adUnitId =  //SettingMgr.current._adState.AndroidInterstitialID();
                           // MAGIC_IOS_INTERSTITIAL_ID;
       OMG_IOS_INSTERSTITIAL_ID;
#elif UNITY_IOS
        string adUnitId = //SettingMgr.current._adState.IOSInterstitialID(); 
        //MAGIC_IOS_INTERSTITIAL_ID;
        OMG_IOS_INSTERSTITIAL_ID;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        _interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        _interstitial.OnAdLoaded += HandleOnLoaded;
        _interstitial.OnAdClosed += HandleOnClosed;
        _interstitial.OnAdOpening += HandleOnOpening;
        _interstitial.OnAdFailedToLoad += HandleOnFailedToLoad;
        // Load the interstitial with the request.
        _interstitial.LoadAd(request);

    }

    static void HandleOnOpening(object sender, EventArgs args)
    {
        TrackAdMob("0");
    }

    static void HandleOnClosed(object sender, EventArgs args)
    {
        TrackAdMob("1");
    }

    private static void HandleOnFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        TrackAdMob("2");
    }

    static void HandleOnLoaded(object sender, EventArgs args)
    {
        TrackAdMob("3");
    }

    


    static InterstitialAd _interstitial;

    public static void TrackAdMob(string idStr)
    {
        TrackAd(idStr, "admobMedi");
    }





		public static int bannerCount = 0;
    public static void ShowAdmobBanner()
    {
        if (_bannerView != null)
        {
			bannerCount++;
            _bannerView.Show();
        }
    }

    public static void HideAdmobBanner()
    {
        if (_bannerView != null)
        {
			bannerCount--;
			if (bannerCount <= 0) 
			{
				_bannerView.Hide();
				bannerCount = 0;
			
			}
           
        }
    }


    public static int downBannerCount = 0;
    public static void ShowDownAdmobBanner()
    {
        if (_downBannerView != null)
        {
            downBannerCount++;
            _downBannerView.Show();
        }
    }

    public static void HideDownAdmobBanner()
    {
        if (_downBannerView != null)
        {
            downBannerCount--;
            if (downBannerCount <= 0)
            {
                _downBannerView.Hide();
                downBannerCount = 0;

            }

        }
    }



    const string ZL_AND_BANNER = "ca-app-pub-7903543313139585/9520800753";
    const string ZL_IOS_BANNER = "";

    static void  InitBanner()
    {
        //#if UNITY_EDITOR
        //        string adUnitId = "unused";
#if UNITY_ANDROID
        string adUnitId =// MAGIC_IOS_BANNER_ID;//ZL_AND_BANNER;
            OMG_IOS_BANNER_ID;
#elif UNITY_IOS

        string adUnitId = //MAGIC_IOS_BANNER_ID;//ZL_AND_BANNER;
          OMG_IOS_BANNER_ID;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        _bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        _bannerView.LoadAd(request);

        _bannerView.Hide();

        AdRequest downRequest = new AdRequest.Builder().Build();
        _downBannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        _downBannerView.LoadAd(downRequest);
        _downBannerView.Hide();





    }

    static BannerView _bannerView;
    static BannerView _downBannerView;


    public static bool IsAdmobInterstitialReady()
    {
		if (_interstitial == null) 
		{
			return false;
		}
        return _interstitial.IsLoaded();
    }





#endregion Admob



    static void TrackAd(string idStr, string plat)
    {
       // MTTracker.Instance.Track(SoliTrack.ads, StatisticsMgr.current.WinsCount(), idStr, plat);
    }



    public static void RegisterAllAd()
    {
        InitBanner();
        //PreloadAdmobInterstitial();

    }








}
