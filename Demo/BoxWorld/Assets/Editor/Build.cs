using UnityEngine;
using UnityEditor;
using System.IO;

public class Builder : MonoBehaviour
{
    static void BuildScene(string scene, string apkDir, string apkName)
    {
        string[] scenes = new string[] {scene};
        Directory.CreateDirectory(apkDir);

        if(System.IO.File.Exists(apkDir + apkName))
        {
            System.IO.File.SetAttributes(apkDir + apkName, System.IO.File.GetAttributes(apkDir + apkName) & ~FileAttributes.ReadOnly);
        }

        BuildPipeline.BuildPlayer(scenes, apkDir + apkName, BuildTarget.Android, BuildOptions.None);
    }
    //
	[MenuItem( "SVR/Build BoxWorld" )]
	static void BuildBoxWorld( )
	{
		try
		{
            Debug.Log("Bulding BoxWorld!");



            PlayerSettings.productName = "boxWorld";
            PlayerSettings.bundleIdentifier = "com.qualcomm.sgs.boxWorld";
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);

            string apkDir = "./build/android/";
            string apkName = "boxWorld.apk";
            string scene = "./Assets/boxWorld.unity";

            BuildScene(scene, apkDir, apkName);
		}
		catch (IOException e)
		{
			Debug.LogError( e.Message );
		}
	}
}
