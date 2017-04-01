using UnityEngine;
using System.Collections;

public class CCJoystick : MonoBehaviour {

	public enum JOYSTICK_DIRECTION
	{
		NONE,
		UP,
		DOWN,
		FRONT,
		BACK
	}
	private JOYSTICK_DIRECTION joystickDirection = JOYSTICK_DIRECTION.NONE;

	/**
	 * 触摸板－>方向按键 回调
	 */ 
	private void onJoystickEvent(JOYSTICK_DIRECTION direction) {
		print ("CCJoystick direction :"+joystickDirection);
		switch (direction) {
		case JOYSTICK_DIRECTION.UP:
			break;
		case JOYSTICK_DIRECTION.DOWN:
			break;
		case JOYSTICK_DIRECTION.FRONT:
			break;
		case JOYSTICK_DIRECTION.BACK:
			break;
		}
	}

	void Update () {
		joystickDirection = JOYSTICK_DIRECTION.NONE;
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			Touch touch = Input.GetTouch (0);
			float x = touch.position.x / Screen.width;
			float y = touch.position.y / Screen.height;
			float dx = x - 0.5f;
			float dy = y - 0.5f;

			if (Mathf.Abs (dx) > Mathf.Abs (dy)) {
				if (dx > 0) {
					joystickDirection = JOYSTICK_DIRECTION.DOWN;
				} else {
					joystickDirection = JOYSTICK_DIRECTION.UP;
				}
			} else {
				if (dy > 0) {
					joystickDirection = JOYSTICK_DIRECTION.FRONT;
				} else {
					joystickDirection = JOYSTICK_DIRECTION.BACK;
				}
			}
			onJoystickEvent (joystickDirection);
		}
	}


}
