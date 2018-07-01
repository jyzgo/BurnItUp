using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_IPHONE || UNITY_ANDROID)
using KochavaUnity;
#endif

public class Kochava : MonoBehaviour {    

    #region Settings

    // Editor-configurable settings
    public string kochavaAppGUID_iOS = "";
    public string kochavaAppGUID_Android = "";
    public string kochavaAppGUID_Kindle = "";
    public bool requestAttributionCallback = false;    
    public bool appAdLimitTracking = false;

    // log level selection
    public enum DebugLogLevel
    {
        none  = 0,
        error = 1,
        warn = 2,
        info = 3,
        debug = 4,
        trace = 5
    }
    public DebugLogLevel logLevel;

    // non-listed settings
    private string kochavaAppId = "";

    #endregion

    // initialize kochava here
    void Start()
    {

#if (UNITY_IPHONE || UNITY_ANDROID)

        // Do not edit this code -- enter your platform-specific appGUIDs into the _Kochava Analytics object within the unity editor
        // (the appropriate app GUID will be chosen based on your build target)
        kochavaAppId = "";
#if UNITY_IPHONE
        kochavaAppId = kochavaAppGUID_iOS;
#elif UNITY_ANDROID
        kochavaAppId = kochavaAppGUID_Android;
        // we need to check if this device is a kindle
        if(kochavaAppGUID_Kindle.Length > 0 && Tracker.Android.isKindle()) kochavaAppId = kochavaAppGUID_Kindle;        
        if (requestAttributionCallback == true)
        {
            // uncomment the line below and pass in your method if you wish to receive an attribution callback for android (see the _SAMPLE_CODE file in this folder for more information)
            // Tracker.Android.SetAttributionHandler(_YOUR_METHOD_HERE_);
        }
#endif

        // initializer with config options (set via Editor, see #region Settings above)
        Tracker.Config.setAppGuid(kochavaAppId);
        Tracker.Config.setLogLevel((int)logLevel);		
        Tracker.Config.setRetrieveAttribution(requestAttributionCallback);   
        Tracker.Config.setAppLimitAdTracking(appAdLimitTracking);        
        Tracker.Initialize();

        // please refer to _SAMPLE_CODE file within Assets > Plugins > Kochava
        // for samples of Kochava public method calls to be used elsewhere in your app

#endif

    }

}
