using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCSystemApi  {

	private static AndroidJavaObject context = null;

	public static AndroidJavaObject getContext(){
		if (context == null) {
			using (AndroidJavaClass player = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
				context = player.GetStatic<AndroidJavaObject> ("currentActivity");
			}
		}
		return context;
	}

	public static void sendCmd(string cmd){
		using (AndroidJavaClass cls = new AndroidJavaClass ("com.coocaa.vr.sdk.CCSystemApi")) {
			cls.CallStatic ("sendCmd",getContext (),cmd);
		}
	}

	public static void reboot(){
		using (AndroidJavaClass cls = new AndroidJavaClass ("com.coocaa.vr.sdk.CCSystemApi")) {
			cls.CallStatic ("reboot",getContext ());
		}
	}

	public static void shutdown(){
		using (AndroidJavaClass cls = new AndroidJavaClass ("com.coocaa.vr.sdk.CCSystemApi")) {
			cls.CallStatic ("shutdown",getContext ());
		}
	}
}