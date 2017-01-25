using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCPmApi {

	public static void bind(){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("bindService");
		}
	}

	public static void unbind(){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("unbind");
		}
	}

	// 使用前，请先提前调用bind来异步绑定服务。绑定成功后调用才有效。
	public static void install(string appName,string apkPath){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("install",appName,apkPath);
		}
	}

	// 未测试
	public static void uninstall(string pkgName){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("uninstall",pkgName);
		}
	}

	public static void hide(){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("cancelInstall");
		}
	}

}
