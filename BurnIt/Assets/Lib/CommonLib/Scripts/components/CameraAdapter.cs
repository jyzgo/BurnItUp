using UnityEngine;
using UnityEngine.UI;

namespace MTUnity
{
	/// <summary>
	/// Camera adapter: Make camera fit to screen resolution.
	/// </summary>
	public class CameraAdapter : MonoBehaviour
	{
		/// <value> Unity2D中每个单位对应的像素 </value>
		public float pixelsPerUnit = 100.0f;
		/// <value> 设计宽度 </value>
		public float designWidth;
		/// <value> 设计高度 </value>
		public float designHeight;
		public CanvasScaler canvasScaler;

		[HideInInspector]
		/// <value> 摄像机区域大小 </value>
		public Rect cameraSize;
        /// <value> 设计相机size</value>
        public float designCameraSize;

		private Camera _mainCamera;

		void Awake ()
		{
			if (designWidth == 0 || designHeight == 0) {//未设置设计尺寸
				Debug.LogWarning ("CameraFitScreenResolution:: The design size not initialized, disable script.");
				this.enabled = false;
				return;
			}

            designCameraSize = designHeight * 0.5f / pixelsPerUnit;

			_mainCamera = GetComponent<Camera> ();
			if (_mainCamera == null) {//挂载此脚本的对象必须是摄像机
				Debug.LogWarning ("CameraFitScreenResolution:: This gameObject isn't a Camera, disable script.");
				this.enabled = false;
			}

			if (canvasScaler != null) {
				canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				canvasScaler.referenceResolution = new Vector2 (designWidth, designHeight);
				canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
				canvasScaler.referencePixelsPerUnit = pixelsPerUnit;
			}

			DeviceOrientationInfo.instance.orientationChanged.AddListener(RefreshCameraAndUI);
		}

        void OnEnable ()
        {
            RefreshCameraAndUI(DeviceOrientationInfo.instance.isLandscape);
        }

		void OnDestroy ()
		{
			DeviceOrientationInfo.instance.orientationChanged.RemoveListener(RefreshCameraAndUI);
		}

        public void RefreshCameraAndUI(bool isLandscape)
        {
            float w, h;

            //默认竖屏
            float curDesignWidth = designWidth;
            float curDesignHeight = designHeight;
            //横屏
            if (Screen.width > Screen.height)
            {
                curDesignWidth = designHeight;
                curDesignHeight = designWidth;
            }

            float screenWidth = Screen.width * 1.0f;
            float screenHeight = Screen.height * 1.0f;

            float aspectRatio = screenWidth / screenHeight;
            //比design宽或者等于design
            if (screenWidth / curDesignWidth >= screenHeight / curDesignHeight)
            {
                w = curDesignWidth / pixelsPerUnit;
                h = (curDesignWidth / aspectRatio) / pixelsPerUnit;
                if (canvasScaler != null)
                {
                    canvasScaler.matchWidthOrHeight = 0;
                }
            }
            //比design长
            else
            {
                w = (curDesignHeight * aspectRatio) / pixelsPerUnit;
                h = curDesignHeight / pixelsPerUnit;
                if (canvasScaler != null)
                {
                    canvasScaler.matchWidthOrHeight = 1;
                }
            }

            _mainCamera.orthographicSize = 0.5f * h;
            cameraSize = new Rect (0, 0, w, h);
        }

		/// <summary>
		/// 获取摄像机根据自适应矫正的比例（可能为0）
		/// </summary>
		public float cameraScales ()
		{
			float standard_width = designWidth;        //初始宽度
			float standard_height = designHeight;       //初始高度
			float device_width = 0f;                //当前设备宽度
			float device_height = 0f;               //当前设备高度
			//获取设备宽高
			device_width = Screen.width;
			device_height = Screen.height;
			//计算宽高比例
			float standard_aspect = standard_width / standard_height;
			float device_aspect = device_width / device_height;
			//计算矫正比例
			float adjustor = 0f;

			if (device_aspect < standard_aspect) {
				adjustor = standard_aspect / device_aspect;
			}
			return adjustor;
		}

	}
}

