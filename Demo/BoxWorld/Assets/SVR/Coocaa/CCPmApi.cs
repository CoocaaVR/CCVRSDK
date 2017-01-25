using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 应用管理接口
 * 使用前，先把ccvrsdk.aar放置在plugins/Andorid目录下，系统版本要求1.2.5以上
 */ 
public class CCPmApi {
	/**
	 * 绑定应用管理服务
	 */ 
	public static void bind(){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("bindService");
		}
	}

	/**
	 * 解绑应用管理服务
	 */ 
	public static void unbind(){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("unbind");
		}
	}

	/**
	 * 应用安装接口
	 * ps:使用前，请先提前调用bind来异步绑定服务。绑定成功后调用才有效。
	 */ 
	public static void install(string appName,string apkPath){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("install",appName,apkPath);
		}
	}

	/**
	 * 应用卸载接口，未经测试
	 * ps:使用前，请先提前调用bind来异步绑定服务。绑定成功后调用才有效。
	 */ 
	public static void uninstall(string pkgName){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("uninstall",pkgName);
		}
	}

	/**
	 * 关闭安装应用弹窗，取消安装
	 */ 
	public static void hide(){
		using(AndroidJavaClass cls = new AndroidJavaClass("com.coocaa.vr.sdk.CCPmApi")){
			cls.CallStatic<AndroidJavaObject> ("getInstance").Call ("cancelInstall");
		}
	}

}
