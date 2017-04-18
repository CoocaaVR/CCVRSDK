using UnityEngine;
using System.Collections;

public class CCMessageBridge : MonoBehaviour {
	/**
	 * 设备按键回调,使用说明： 
	 * 把该脚本挂在SvrCamera上即可
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

	/**
	 * data为json格式的电量信息
	 */
	public void getBatteryInfo(string data){
	}

}
