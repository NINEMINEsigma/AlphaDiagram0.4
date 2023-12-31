using UnityEditor;
using System.IO;
using AD.BASE;
using UnityEngine;

public class AssetBuldle : Editor
{
    [MenuItem("Tools/CreatAssetBundle for Android")]
    static void CreatAssetBundle()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "AB");
        FileC.TryCreateDirectroryOfFile(path + "/XX.X");
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.Android);
        UnityEngine.Debug.Log("Android Finish!");
    }

    [MenuItem("Tools/CreatAssetBundle for IOS")]
    static void BuildAllAssetBundlesForIOS()
    {
        string dirName = "AssetBundles/IOS";
        if (!Directory.Exists(dirName))
        {
            Directory.CreateDirectory(dirName);
        }
        BuildPipeline.BuildAssetBundles(dirName, BuildAssetBundleOptions.None, BuildTarget.iOS);
        UnityEngine.Debug.Log("IOS Finish!");

    }


    [MenuItem("Tools/CreatAssetBundle for Win")]
    static void CreatPCAssetBundleForwINDOWS()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "AB");
        FileC.TryCreateDirectroryOfFile(path+"/XX.X");
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        UnityEngine.Debug.Log("Windows Finish!");
    }
}
