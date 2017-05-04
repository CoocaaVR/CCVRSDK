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


/// <summary>
	/// Sends the LED.设置呼吸灯颜色以及状态
	/// color:red、blue、green、colors分别代表红、绿、蓝、彩色
	/// mode:1循环、2常亮、3亮一秒 其他值表示循环,默认循环模式
	/// 注：彩色仅循环模式
/// </summary>
/// <param name="color">red</param>
/// <param name="mode">1</param>
	public static void sendLED(string color,int mode){
		using (AndroidJavaClass cls = new AndroidJavaClass ("com.coocaa.vr.sdk.CCSystemApi")) {
			cls.CallStatic ("sendLED",getContext (),color,mode);
		}
	}
	/// <summary>
	/// Requests the battery info.
	/// </summary>
	public static void requestBatteryInfo(){
		using (AndroidJavaClass cls = new AndroidJavaClass ("com.coocaa.vr.sdk.CCSystemApi")) {
			cls.CallStatic ("getBatteryInfo",getContext ());
		}
	}

	/// <summary>
	/// Gets the mac address.
	/// </summary>
	/// <returns>The mac address.</returns>
	public static string getMacAddress(){
		string macAddress;
		using (AndroidJavaClass cls = new AndroidJavaClass ("com.coocaa.vr.sdk.CCSystemApi")) {
			macAddress = cls.CallStatic <string>("getMacAddress", getContext ());
		}
		return macAddress;
	}

	/// <summary>
	/// Sets the VOL.
	/// maxVolume = 15
	/// </summary>
	/// <param name="volume">Volume.</param>
	public static void setVOL(int volume){
		using (AndroidJavaClass cls = new AndroidJavaClass ("com.coocaa.vr.sdk.CCSystemApi")) {
			cls.CallStatic ("setVOL",getContext (),volume);
		}
	}
}