using UnityEngine;
using System;
using System.Collections;

public abstract class SvrPlugin
{
	private static SvrPlugin instance;

	public static SvrPlugin Instance
	{
		get
		{
			if (instance == null)
			{
				if(!Application.isEditor && Application.platform == RuntimePlatform.Android)
				{
					instance = SvrPluginAndroid.Create();
				}
				else
				{
					instance = SvrPluginWin.Create();
				}
			}

			return instance;
		}
	}

	public enum PerfLevel
	{
        kPerfSystem = 0,
        kPerfMaximum = 1,
		kPerfNormal = 2,
		kPerfMinimum = 3
	}

    public enum TrackingMode
    {
        kTrackingOrientation = 1,
        kTrackingPosition = 2
    }

	public struct DeviceInfo
	{
		public int 		displayWidthPixels;
		public int    	displayHeightPixels;
		public float  	displayRefreshRateHz;
		public int    	targetEyeWidthPixels;
		public int    	targetEyeHeightPixels;
		public float  	targetFovXRad;
		public float  	targetFovYRad;
	}

	public virtual IEnumerator Initialize (){ yield break; }
	public virtual IEnumerator BeginVr(){ yield break; }
	public virtual void EndVr(){}
	public virtual void EndEye(){}
    public virtual void SetTrackingMode(TrackingMode mode) { }
    public virtual void SetPerformanceLevels(int newCpuPerfLevel, int newGpuPerfLevel) { }
    public virtual void RecenterTracking() { }
	public virtual void SubmitFrame(int frameIndex, int leftEyeTextureId, int rightTextureId){}
	public virtual void GetPredictedPose (ref Quaternion orientation, ref Vector3 position)
	{
		orientation =  Quaternion.identity;
		position = Vector3.zero;
	}
	public abstract DeviceInfo GetDeviceInfo ();
	public virtual void Shutdown()
    {
        SvrPlugin.instance = null;
    }
}
