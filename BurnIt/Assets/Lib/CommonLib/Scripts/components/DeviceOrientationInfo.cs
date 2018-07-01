using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
/// <summary>
/// 当前的屏幕信息，横屏还是竖屏（编辑器模式，以当前屏幕宽高比来算）
/// 以及屏幕旋转事件，外部可注册
/// 注意：
/// 1.该类为单例类，没有对其进行主动销毁
/// 2.基于1，屏幕旋转回调不需要的时候，别忘了添加对应的RemoveListener，否则可能带来内存泄漏
/// </summary>
public class DeviceOrientationInfo : MonoBehaviour {
    //Text text;

    static DeviceOrientationInfo _instance;

    public static DeviceOrientationInfo instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DeviceOrientationInfo>();
                if (_instance != null)
                    return _instance;
                GameObject go = new GameObject("DeviceOrientationInfo");
                _instance = go.AddComponent<DeviceOrientationInfo>();
                go.hideFlags = HideFlags.DontSave;
            }
            return _instance;
        }
    }

    bool _isLandscape;
    public OrientationChangedEvent orientationChanged = new OrientationChangedEvent();
    public bool isLandscape { get { return _isLandscape; } }

    void Awake()
    {
        _instance = this;
        //text = GetComponent<Text>();
        //text.enabled = false;
        //DontDestroyOnLoad(gameObject);
        #if UNITY_EDITOR
        if (Screen.width > Screen.height)
        {
            _isLandscape = true;
        }
        else
        {
            _isLandscape = false;
        }
        #else 
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            _isLandscape = true;
        }
        else
        {
            _isLandscape = false;
        }
        #endif
        orientationChanged.AddListener(OrientationChangedCallBack);
    }

    void OnDestroy()
    {
        orientationChanged.RemoveAllListeners();
    }



    void OrientationChangedCallBack(bool isLandscape)
    {
        //ShowErrorText(_isLandscape.ToString());
    }

    /*
    Vector3 initScale;
    Vector3 initPos;
    float initCameraSize;
    
    public void RefreshLevelSceneRotation()
    {
        if (initPos == Vector3.zero)
        {
            initPos = this.transform.position;
            initScale = this.transform.localScale;
            initCameraSize = Camera.main.orthographicSize;
        }

        if (_isLandscape)
        {
            Vector3 cameraPos = GridCamera.current.cameraHolder.transform.position;

            Vector3 gridCenterPoint = new Vector3(cameraPos.x, cameraPos.y + initPos.y, initPos.z);
            Vector3 gridCenterPointNew = new Vector3(cameraPos.x + cameraPos.y - gridCenterPoint.y, cameraPos.y, initPos.z);
            this.transform.position += gridCenterPointNew - gridCenterPoint;

            Camera.main.orthographicSize *= Screen.height * 1.0f / Screen.width;
        }
        else
        {
            transform.position = initPos;
            transform.localScale = initScale;
            Camera.main.orthographicSize = initCameraSize;
        }
    }*/



    void ShowErrorText(string message)
    {
        //text.enabled = true;
        //text.text = message;
    }

    // Update is called once per frame
    void Update () {
        #if UNITY_EDITOR
        if (_isLandscape)
        {
            if (Screen.height > Screen.width)
            {
                _isLandscape = false;
                orientationChanged.Invoke(_isLandscape);
            }
        }
        else
        {
            if (Screen.width > Screen.height)
            {
                _isLandscape = true;
                orientationChanged.Invoke(_isLandscape);
            }
        }
        #else
        if (Screen.orientation == ScreenOrientation.Unknown)
            return;

        if (_isLandscape)
        {
            if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                _isLandscape = false;
                orientationChanged.Invoke(_isLandscape);
            }
        }
        else
        {
            if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                _isLandscape = true;
                orientationChanged.Invoke(_isLandscape);
            }
        }
        #endif

    }

    public class OrientationChangedEvent : UnityEvent<bool> {}


}
