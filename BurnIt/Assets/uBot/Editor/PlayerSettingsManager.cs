using System.Collections;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 播放器设置管理器
/// 可拓展
/// </summary>
public class PlayerSettingsManager
{
    [MenuItem("PlayerSettings/Android")]
    static void SetAndroid()
    {
        AssignPlayerSettingsForAndroid();
    }

    [MenuItem("PlayerSettings/iOS")]
    static void SetiOS()
    {
        AssignPlayerSettingsForiOS();
    }

    /// <summary>
    /// 指定安卓播放器设置
    /// </summary>
    public static void AssignPlayerSettingsForAndroid()
    {
        PlayerSettings.applicationIdentifier = PlayerSettingsConfig.BundleIdentifier_Android;
        PlayerSettings.bundleVersion = PlayerSettingsConfig.BundleVersion_Android;
        //PlayerSettings.Android.bundleVersionCode = int.Parse(PlayerSettingsConfig.BundleVersionCode_Android);
    }

    /// <summary>
    /// 指定苹果播放器设置
    /// </summary>
    public static void AssignPlayerSettingsForiOS()
    {
        PlayerSettings.applicationIdentifier = PlayerSettingsConfig.BundleIdentifier_iOS;
        PlayerSettings.bundleVersion = PlayerSettingsConfig.BundleVersion_iOS;
        PlayerSettings.iOS.buildNumber = PlayerSettingsConfig.BundleVersionCode_iOS;
    }
}

/// <summary>
/// 播放器设置配置类
/// </summary>
public class PlayerSettingsConfig
{
    /// <summary>
    /// 安卓包名
    /// </summary>
    public static string BundleIdentifier_Android = "com.solitaire.omg";
    /// <summary>
    /// 安卓包版本号
    /// </summary>
    public static string BundleVersion_Android = "1.0";
    /// <summary>
    /// 安卓包版本代码
    /// </summary>
    public static string BundleVersionCode_Android = "11";

    /// <summary>
    /// 苹果包名
    /// </summary>
    public static string BundleIdentifier_iOS = "com.tastytreats.fruitmatch.casual";
    /// <summary>
    /// 苹果包版本号
    /// </summary>
    public static string BundleVersion_iOS = "1.0";
    /// <summary>
    /// 苹果包版本代码
    /// </summary>
    public static string BundleVersionCode_iOS = "1";
}