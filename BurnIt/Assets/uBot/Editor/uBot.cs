using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
#endif

public class uBot {

	static bool _useAppsee = false;

	static readonly string[] scenes = {
        "Assets/Scene/Launch.unity",

        "Assets/Scene/Main.unity"
            
	};

	static void BuildAndroid() {
		string targetPath = "/tmp/build/uBot.apk";
		PlayerSettings.Android.keystorePass = "123456";
		PlayerSettings.Android.keyaliasPass = "123456";
        PlayerSettingsManager.AssignPlayerSettingsForAndroid();
        //PlayerSettingsManager.Ass

        BuildPipeline.BuildPlayer (scenes, targetPath, BuildTarget.Android, BuildOptions.None);
	}

	static void BuildiOS() {
		if(_useAppsee)
			CopyAppseeFramework();
		string targetPath = "/tmp/build/uBot";
		BuildPipeline.BuildPlayer (scenes, targetPath, BuildTarget.iOS, BuildOptions.None);
		if(_useAppsee)
			LinkAppseeLibrariesIOS(targetPath);
	}

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
        Debug.Log("[OnPostprocessBuild] target = " + target + ", pathToBuiltProject = " + pathToBuiltProject);
        if (target == BuildTarget.iOS) {
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            var typesKey = "CFBundleURLTypes";
            PlistElementDict rootDict = plist.root;
            var types = rootDict[typesKey];
            if (types == null)
            {
                types = rootDict.CreateArray(typesKey);
            }
            var ta = types.AsArray();
            var newScheme = ta.AddDict();
            newScheme.SetString("CFBundleURLName", "com.solitaire.omg");
            var schems = newScheme.CreateArray("CFBundleSchemes");
            schems.AddString("omgsolitaire");

           



            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
#if UNITY_EDITOR_OSX
            string xcodeProjectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject xcodeProject = new PBXProject();
            xcodeProject.ReadFromFile(xcodeProjectPath);

            // setting bitcode to No
            string xcodeTarget = xcodeProject.TargetGuidByName("Unity-iPhone");
            xcodeProject.SetBuildProperty(xcodeTarget, "ENABLE_BITCODE", "NO");
            xcodeProject.AddBuildProperty(xcodeTarget, "OTHER_LDFLAGS", "-ObjC");
            xcodeProject.AddBuildProperty(xcodeTarget, "CLANG_ENABLE_MODULES", "YES");

            xcodeProject.AddFrameworkToProject(xcodeTarget, "AdSupport.framework", true);
             xcodeProject.AddFrameworkToProject(xcodeTarget, "iAd.framework", true);
			xcodeProject.AddFrameworkToProject(xcodeTarget, "GoogleMobileAds.framework", false);

            // Save the changes to Xcode project file.
            xcodeProject.WriteToFile(xcodeProjectPath);

#endif
        }
    }

	public static void CopyAppseeFramework()
	{
		string srcDir = Application.dataPath + "/Editor/appsee/Appsee.framework";
		string destDir = Application.dataPath + "/Plugins/iOS";
		CopyFolder(srcDir, destDir);
	}

	public static void LinkAppseeLibrariesIOS(string path)
	{
//		string srcDir = Application.dataPath + "/Editor/appsee/Appsee.framework";
//		string destDir = "/tmp/build/uBot/Frameworks";
//		CopyFolder(srcDir, destDir);

		File.Copy(Application.dataPath + "/Editor/appsee/DisplayManager.mm", "/tmp/build/uBot/Classes/Unity/DisplayManager.mm", true);
		File.Copy(Application.dataPath + "/Editor/appsee/UnityAppController.mm", "/tmp/build/uBot/Classes/UnityAppController.mm", true);

		string projectFile = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
		string contents = File.ReadAllText(projectFile);



		string pbxStr = "/* Begin PBXBuildFile section */";
		int index = contents.IndexOf(pbxStr) + pbxStr.Length;
		string pbxInsertStr = "\n\t\tAED98FBC1CDB6B6A00D4CDAF /* libz.tbd in Frameworks */ = {isa = PBXBuildFile; fileRef = AED98FBB1CDB6B6A00D4CDAF /* libz.tbd */; };";
		contents = contents.Insert(index, pbxInsertStr);

		pbxStr = "/* Begin PBXFileReference section */";
		index = contents.IndexOf(pbxStr) + pbxStr.Length;
		pbxInsertStr = "\n\t\tAED98FBB1CDB6B6A00D4CDAF /* libz.tbd */ = {isa = PBXFileReference; lastKnownFileType = \"sourcecode.text-based-dylib-definition\"; name = libz.tbd; path = usr/lib/libz.tbd; sourceTree = SDKROOT; };";
		contents = contents.Insert(index, pbxInsertStr);

		pbxStr = "/* Begin PBXFrameworksBuildPhase section */";
		index = contents.IndexOf(pbxStr) + pbxStr.Length;
		pbxInsertStr = "\n\t\t\t\tAED98FBC1CDB6B6A00D4CDAF /* libz.tbd in Frameworks */,";
		string inserKey = "files = (";
		int insertIndex = contents.IndexOf(inserKey, index) + inserKey.Length;
		contents = contents.Insert(insertIndex, pbxInsertStr);

		pbxStr = "/* Begin PBXGroup section */";
		index = contents.IndexOf(pbxStr) + pbxStr.Length;
		pbxInsertStr = "\n\t\t\t\tAED98FBB1CDB6B6A00D4CDAF /* libz.tbd */,";
		inserKey = "/* CoreText.framework */,";
		insertIndex = contents.IndexOf(inserKey, index) + inserKey.Length;
		contents = contents.Insert(insertIndex, pbxInsertStr);

		File.WriteAllText(projectFile, contents);


		string destDir = Application.dataPath + "/Plugins/iOS/Appsee.framework";
		Directory.Delete(destDir, true);
		string destFile = Application.dataPath + "/Plugins/iOS/Appsee.framework.meta";
		File.Delete(destFile);
	}
//	[MenuItem("testapp/testxcodeproj")]
//	public static void testapp()
//	{
//		//LinkAppseeLibrariesIOS("/tmp/build/uBot");
//		string destDir = Application.dataPath + "/Plugins/iOS/Appsee.framework";
//		Directory.Delete(destDir, true);
//		string destFile = Application.dataPath + "/Plugins/iOS/Appsee.framework.meta";
//		File.Delete(destFile);
//	}

	public static void CopyFolder(string strFromPath,string strToPath)
	{
		//如果源文件夹不存在，则创建
		if (!Directory.Exists(strFromPath))
		{ 
			Directory.CreateDirectory(strFromPath);
		}
		//取得要拷贝的文件夹名
		string strFolderName = strFromPath.Substring(strFromPath.LastIndexOf("/") +
			1,strFromPath.Length - strFromPath.LastIndexOf("/") - 1);
		//如果目标文件夹中没有源文件夹则在目标文件夹中创建源文件夹
		if (!Directory.Exists(strToPath + "/" + strFolderName))
		{ 
			Directory.CreateDirectory(strToPath + "/" + strFolderName);
		}
		//创建数组保存源文件夹下的文件名
		string[] strFiles = Directory.GetFiles(strFromPath);
		//循环拷贝文件
		for(int i = 0;i < strFiles.Length;i++)
		{
			//取得拷贝的文件名，只取文件名，地址截掉。
			string strFileName = strFiles[i].Substring(strFiles[i].LastIndexOf("/") + 1,strFiles[i].Length - strFiles[i].LastIndexOf("/") - 1);
			//开始拷贝文件,true表示覆盖同名文件
			File.Copy(strFiles[i],strToPath + "/" + strFolderName + "/" + strFileName,true);
		}
		//创建DirectoryInfo实例
		DirectoryInfo dirInfo = new DirectoryInfo(strFromPath);
		//取得源文件夹下的所有子文件夹名称
		DirectoryInfo[] ZiPath = dirInfo.GetDirectories();
		for (int j = 0;j < ZiPath.Length;j++)
		{
			//获取所有子文件夹名
			string strZiPath = ZiPath[j].ToString(); 
			//把得到的子文件夹当成新的源文件夹，从头开始新一轮的拷贝
			CopyFolder(strZiPath,strToPath + "/" + strFolderName);
		}
	}
}
