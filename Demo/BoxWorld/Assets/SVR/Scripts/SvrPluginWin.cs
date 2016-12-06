using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class SvrPluginWin : SvrPlugin
{
	GameObject 	cameraRig;
	SvrEye []	eyes = null;

	public static SvrPluginWin Create()
	{
		return new SvrPluginWin ();
	}

	private SvrPluginWin() {}

	public override IEnumerator Initialize()
	{
		cameraRig = GameObject.FindGameObjectWithTag("SvrCamera");
		if (cameraRig == null)
		{
			Debug.Log("Gameobject with tag SvrCamera not found!");
			yield break;
		}
	}

	public override IEnumerator BeginVr()
	{
		if(eyes == null)
		{
			eyes = cameraRig.GetComponentsInChildren<SvrEye> ();
			if (eyes == null)
			{
				Debug.Log("Components with SvrEye not found!");
			}
		}

        yield break;
    }

    public override void GetPredictedPose(ref Quaternion orientation, ref Vector3 position)
	{
		orientation = Quaternion.identity;
		position = Vector3.zero;
	}

	public override DeviceInfo GetDeviceInfo()
	{
		DeviceInfo info 			= new DeviceInfo();

		info.displayWidthPixels 	= Screen.width;
		info.displayHeightPixels 	= Screen.height;
		info.displayRefreshRateHz 	= 60.0f;
		info.targetEyeWidthPixels 	= Screen.width / 2;
		info.targetEyeHeightPixels 	= Screen.height;
		info.targetFovXRad			= Mathf.Deg2Rad * 90;
		info.targetFovYRad			= Mathf.Deg2Rad * 90;

		return info;
	}

	public override void SubmitFrame(int frameIndex, int leftEyeTextureId, int rightTextureId)
	{
		RenderTexture.active = null;
		GL.Clear (false, true, Color.black);

		Rect leftRect = new Rect (25.0f, 0.0f, Screen.width * 0.5f - 25.0f, Screen.height);
		Graphics.DrawTexture (leftRect,eyes [0].GetComponent<Camera> ().targetTexture);

		Rect rightRect = new Rect (Screen.width * 0.5f + 25.0f, 0.0f, Screen.width * 0.5f - 50.0f, Screen.height);
		Graphics.DrawTexture (rightRect, eyes [1].GetComponent<Camera> ().targetTexture);
	}

	public override void Shutdown()
	{
        base.Shutdown();
    }
}
