using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace MTUnity {

	/**
	 * Values do not exceed 1 digit, otherwise you need to change the parse code below
	 * Also do not change the values of the enum items, as they are used in the native code
	 */
	public enum DeviceIDType {
		UnityID = 0,
		IDFA = 1,
		IDFV = 2,
		AAID = 3
	}

	public class DeviceID {
		public DeviceIDType type;
		public string id;
	}

	public class NativeUtil {

		#region Device ID fetching

		public static DeviceID GetDeviceIdentifier() {
			DeviceID deviceId = new DeviceID ();
			string raw = GetDeviceIdentifierRaw ();
			deviceId.type = (DeviceIDType)int.Parse (raw.Substring (0, 1));
			deviceId.id = raw.Substring (1);

			// fallback
			if (deviceId.id == "") {
				deviceId.id = SystemInfo.deviceUniqueIdentifier;
				deviceId.type = DeviceIDType.UnityID;
			}

			return deviceId;
		}


		/// Raw functions: first character of the returned string is ID type, as defined in the DeviceIDType enum

		#if UNITY_ANDROID && !UNITY_EDITOR

		private static string GetDeviceIdentifierRaw() {
			
			AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");

			// Check availability
			AndroidJavaClass availCls = new AndroidJavaClass ("com.google.android.gms.common.GoogleApiAvailability");
			AndroidJavaObject avail = availCls.CallStatic<AndroidJavaObject> ("getInstance");
			int code = avail.Call<int> ("isGooglePlayServicesAvailable", currentActivity);
			if (code != 0) { // Google Play Services not available
				Debug.LogWarning ("Google Play Services is not available!");
				return (int)DeviceIDType.UnityID + "";
			}

			AndroidJavaClass client = new AndroidJavaClass ("com.google.android.gms.ads.identifier.AdvertisingIdClient");
			AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject> ("getAdvertisingIdInfo",currentActivity);

			string advertisingID = adInfo.Call<string> ("getId");
			bool limitTracking = (adInfo.Call<bool> ("isLimitAdTrackingEnabled"));

			// Strictly speeking, we should not use AAID if limitTracking is true
			return (int)DeviceIDType.AAID +  advertisingID;
		}

		#elif UNITY_IOS && !UNITY_EDITOR

		[DllImport ("__Internal")]
		private static extern string GetDeviceIdentifierRaw();

		#else

		private static string GetDeviceIdentifierRaw() {
			return (int)DeviceIDType.UnityID +  SystemInfo.deviceUniqueIdentifier;
		}

		#endif

		#endregion
	}

}