using UnityEngine;
using System.Collections;

public class CCMessageBridge : MonoBehaviour {
	/**
	 * 设备按键回调,使用说明： 
	 * 1、把该脚本挂在SvrCamera上；
	 * 2、把ccvrsdk.aar放置在plugins/Andorid目录下；
	 * 3、修改AndroiodManifest，把“com.unity3d.player.UnityPlayerActivity”改成为"com.coocaa.vr.sdk.CCVrSdkActivity"
	 * 4、完成
	 */ 
	public void onKeyDown(string key){
		if ("KEYCODE_VOLUME_DOWN".Equals (key)) {
			print ("ccvr KEYCODE_VOLUME_DOWN");
		} else if("KEYCODE_VOLUME_UP".Equals (key)){
			print ("ccvr KEYCODE_VOLUME_UP");
		} else if("KEYCODE_BACK".Equals (key)){
			print ("ccvr KEYCODE_BACK");
		}
	}
}
