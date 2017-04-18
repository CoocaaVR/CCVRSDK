using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class SvrPluginAndroid : SvrPlugin
{
	[DllImport("svrplugin")]
	private static extern IntPtr GetRenderEventFunc();
	[DllImport("svrplugin")]
	private static extern bool SvrIsInitialized();
    [DllImport("svrplugin")]
    private static extern bool SvrCanBeginVR();
    [DllImport("svrplugin")]
	private static extern void SvrInitializeEventData(IntPtr activity);
	[DllImport("svrplugin")]
	private static extern void SvrSubmitFrameEventData(int frameIndex,
	                                                   int leftEyeTextureId,
	                                                   int rightEyeTextureId);
	[DllImport("svrplugin")]
	private static extern void SvrSubmitFrameEventDataEx(int frameIndex,
		int leftEyeTextureId,
		int rightEyeTextureId,
		int leftOverlayBufferId,
		int rightOverlayBufferId);
	[DllImport("svrplugin")]
	private static extern void SvrSetTrackingModeEventData(int mode);
	
	[DllImport("svrplugin")]
	private static extern void SvrSetPerformanceLevelsEventData(int newCpuPerfLevel, 
	                                                            int newGpuPerfLevel);
	[DllImport("svrplugin")]
	private static extern void SvrGetPredictedPose(ref float rx,
	                                               ref float ry,
	                                               ref float rz,
	                                               ref float rw,
												   ref float px,
												   ref float py,
												   ref float pz);
	[DllImport("svrplugin")]
	private static extern void SvrGetDeviceInfo(ref int displayWidthPixels,
	                                            ref int displayHeightPixels,
	                                            ref float displayRefreshRateHz,
	                                            ref int targetEyeWidthPixels,
	                                            ref int targetEyeHeightPixels,
	                                            ref float targetFovXRad,
	                                       		ref float targetFovYRad);

	int leftLayoutBufferId,rightLayoutBufferId;

	private enum RenderEvent
	{
		Initialize,
		BeginVr,
		EndVr,
		EndEye,
		SubmitFrame,
		Shutdown,
		RecenterTracking,
		SetTrackingMode,
		SetPerformanceLevels
	};

	public static SvrPluginAndroid Create()
	{
		if(Application.isEditor)
		{
			Debug.LogError("SvrPlugin not supported in unity editor!");
			throw new InvalidOperationException();
		}
		return new SvrPluginAndroid ();
	}

	private SvrPluginAndroid() {
		using (AndroidJavaClass cls = new AndroidJavaClass ("com.coocaa.vr.sdk.CCSystemApi")) {
			leftLayoutBufferId = cls.CallStatic<int> ("getLeftFocusId");
			rightLayoutBufferId = cls.CallStatic<int> ("getRightFocusId");
		}
	}

	private void IssueEvent(RenderEvent e)
	{
		// Queue a specific callback to be called on the render thread
		GL.IssuePluginEvent(GetRenderEventFunc(), (int)e);
	}

	public override IEnumerator Initialize()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		SvrInitializeEventData(activity.GetRawObject());
#endif

		IssueEvent (RenderEvent.Initialize);
		yield return new WaitUntil (() => SvrIsInitialized () == true);
	}

	public override IEnumerator BeginVr()
	{
        yield return new WaitUntil(() => SvrCanBeginVR() == true);
        IssueEvent (RenderEvent.BeginVr);
	}

	public override void EndVr()
	{
		IssueEvent (RenderEvent.EndVr);
	}

	public override void EndEye()
	{
		IssueEvent (RenderEvent.EndEye);
	}

    public override void SetTrackingMode(TrackingMode mode)
    {
        SvrSetTrackingModeEventData((int)mode);
		IssueEvent (RenderEvent.SetTrackingMode);
    }

    public override void SetPerformanceLevels(int newCpuPerfLevel, int newGpuPerfLevel)
    {
        SvrSetPerformanceLevelsEventData((int)newCpuPerfLevel, (int)newGpuPerfLevel);
		IssueEvent (RenderEvent.SetPerformanceLevels);
    }

    public override void RecenterTracking()
	{
		IssueEvent (RenderEvent.RecenterTracking);
	}

	public override void GetPredictedPose(ref Quaternion orientation, ref Vector3 position)
	{
		orientation = Quaternion.identity;
		position = Vector3.zero;

		SvrGetPredictedPose(ref orientation.x, ref orientation.y, ref orientation.z, ref orientation.w,
							ref position.x, ref position.y, ref position.z);

		orientation.z = -orientation.z;
        position.x = -position.x;
        position.y = -position.y;
	}

	public override DeviceInfo GetDeviceInfo()
	{
		DeviceInfo info = new DeviceInfo();

		SvrGetDeviceInfo (ref info.displayWidthPixels,
		                  ref info.displayHeightPixels,
		                  ref info.displayRefreshRateHz,
		                  ref info.targetEyeWidthPixels,
		                  ref info.targetEyeHeightPixels,
		                  ref info.targetFovXRad,
		                  ref info.targetFovYRad);

		return info;
	}

	public override void SubmitFrame(int frameIndex, int leftEyeTextureId, int rightTextureId)
	{
		if (SvrManager.showLayoutBuffer) {
			SvrSubmitFrameEventDataEx (frameIndex, leftEyeTextureId, rightTextureId, leftLayoutBufferId, rightLayoutBufferId);
		} else {
			SvrSubmitFrameEventDataEx (frameIndex, leftEyeTextureId, rightTextureId,0,0);
		}
		IssueEvent (RenderEvent.SubmitFrame);
	}

	public override void Shutdown()
	{
        EndVr();
        IssueEvent (RenderEvent.Shutdown);

        base.Shutdown();
	}
}
