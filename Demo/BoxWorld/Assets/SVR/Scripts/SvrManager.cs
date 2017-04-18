using UnityEngine;
using System;
using System.Collections;

public class SvrManager : MonoBehaviour
{
	public enum eRenderTextureDepth
	{
		k16 = 16,
		k24 = 24
	};

	public enum eRenderTextureAntiAliasing
	{
		k1 = 1,
		k2 = 2,
		k4 = 4
	};

    public enum ePerfLevel
    {
        System = 0,
        Minimum = 1,
        Medium = 2,
        Maximum = 3
    };


	public eRenderTextureDepth renderTextureDepth = eRenderTextureDepth.k24;
	public eRenderTextureAntiAliasing renderTextureAntiAliasing = eRenderTextureAntiAliasing.k1;
	public bool hdr	= false;
	public bool	trackPosition= false;
    public float trackPositionScale = 1;
	public float headHeight	= 0.0750f;
	public float headDepth = 0.0805f; 
	public ePerfLevel cpuPerfLevel = ePerfLevel.System;
    public ePerfLevel gpuPerfLevel = ePerfLevel.System;

	public Camera leftCamera;
	public Camera rightCamera;
	public Transform head;
   
	private int	frameCount = 0;
	private static 	WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
	private SvrPlugin plugin = null;

    public float interPupilDistance = 0.064f;

    private bool initialized = false;
	private SvrEye[] eyes = new SvrEye[(int)SvrEye.Side.Count];
    private bool disableInput = false;
    private bool isPaused = false;
    private bool isBeginVR = false;

	public static bool showLayoutBuffer = false;

    public bool DisableInput
    {
        get { return disableInput; }
        set { disableInput = value; }
    }

	void Awake()
	{
		if (!ValidateReferencedComponents ())
		{
			enabled = false;
			return;
		}	
		print ("CCVRSDK version : " + CCVR.version);
	}

	bool ValidateReferencedComponents()
	{
		plugin = SvrPlugin.Instance;
		if(plugin == null)
		{
			Debug.LogError("Svr Plugin failed to load. Disabling...");
			return false;
		}

		if(head == null)
		{
			Debug.LogError("Required head gameobject not found! Disabling...");
			return false;
		}

		if(leftCamera == null || rightCamera == null)
		{
			Debug.LogError("Required eye components are missing! Disabling...");
			return false;
		}

		return true;
	}

	// Use this for initialization
	IEnumerator Start ()
	{
		Debug.LogError ("### Start enter.");
		yield return StartCoroutine(Initialize());
        if(isScreenOn())
        {
            yield return StartCoroutine(plugin.BeginVr());

            // Do this AFTER BeginVr() because it sets performance levels to it's own defaults
            plugin.SetPerformanceLevels((int)cpuPerfLevel, (int)gpuPerfLevel);

            StartCoroutine(SubmitFrame());

            isBeginVR = true;
			Debug.LogError ("### Start enter. isBeginVR = true");
        }
		
	}

	private IEnumerator Initialize()
	{
		// Plugin must be initialized OnStart in order to properly
		// get a valid surface
        GameObject mainCameraGo = GameObject.FindWithTag("MainCamera");
        if (mainCameraGo)
        {
            mainCameraGo.SetActive(false);

            Debug.Log("Camera with MainCamera tag found.");
            if (!disableInput)
            {
                Debug.Log("Will use translation and orientation from the MainCamera.");
                transform.position = mainCameraGo.transform.position;
                transform.rotation = mainCameraGo.transform.rotation;
            }

            Debug.Log("Disabling Camera with MainCamera tag");
        }

        GL.Clear(false, true, Color.black);

		yield return StartCoroutine(plugin.Initialize ());

		InitializeEyes ();

        if (trackPosition)
        {
            plugin.SetTrackingMode(SvrPlugin.TrackingMode.kTrackingPosition);
        }
        else
        {
            plugin.SetTrackingMode(SvrPlugin.TrackingMode.kTrackingOrientation);
        }

		initialized = true;
		Debug.Log("Svr initialized!");
	}

	private void InitializeEyes()
	{
		eyes [0] = leftCamera.gameObject.AddComponent<SvrEye> ();
		eyes [0].side = SvrEye.Side.Left;
		eyes [1] = rightCamera.gameObject.AddComponent<SvrEye> ();
		eyes [1].side = SvrEye.Side.Right;

		leftCamera.hdr = hdr;
		rightCamera.hdr = hdr;

		leftCamera.enabled = true;
		rightCamera.enabled = true;

		if(hdr && renderTextureAntiAliasing != eRenderTextureAntiAliasing.k1)
		{
			Debug.LogWarning("Antialiasing not supported when HDR is enabled. Disabling antiAliasing...");
			renderTextureAntiAliasing = eRenderTextureAntiAliasing.k1;
		}

		SvrPlugin.DeviceInfo info = plugin.GetDeviceInfo ();

		foreach(SvrEye eye in eyes)
		{
			Vector3 eyePos;
			eyePos.x = (eye.side == SvrEye.Side.Left ? -0.5f : 0.5f) * interPupilDistance + head.transform.localPosition.x;
			eyePos.y = (!trackPosition ? headHeight : 0) + head.transform.localPosition.y;
			eyePos.z = (!trackPosition ? -headDepth : 0) + head.transform.localPosition.z;

			eye.transform.localPosition = eyePos;
			eye.format					= hdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
			eye.textureSize 			= new Vector2(info.targetEyeWidthPixels, info.targetEyeHeightPixels);
			eye.fov 					= new Vector2(info.targetFovXRad * Mathf.Rad2Deg, info.targetFovYRad * Mathf.Rad2Deg);
			eye.depth 					= (int)renderTextureDepth;
			eye.antiAliasing 			= (int)renderTextureAntiAliasing;	// hdr not supported with antialiasing
			eye.OnPostRenderListener 	= OnPostRenderListener;
		}
	}

	IEnumerator SubmitFrame ()
	{
		while(true)
		{
			yield return waitForEndOfFrame;
			
			plugin.SubmitFrame(frameCount, eyes[0].TextureId, eyes[1].TextureId);
			
			frameCount++;
		}
	}
	
	public void RecenterTracking()
	{
		plugin.RecenterTracking();
	}

	void OnPostRenderListener ()
	{
		plugin.EndEye ();
	}

    private void OnApplicationPause(bool pause)
    {
		print ("### OnApplicationPause " + pause);
		SetPause(pause);

    }

    public void SetPause(bool pause)
	{
        if (!initialized || isPaused == pause)
			return;

		print ("### SetPause " + pause);
        if (pause)
		{
			OnPause();
		}
		else
		{
			StartCoroutine(OnResume());
		}
    }

    void OnPause()
	{
		Debug.LogError ("### OnPause enter");
        isPaused = true;
        if(!isBeginVR)
        {
			Debug.LogError ("### OnPause enter. isBeginVR = false");
            return;
        }
		StopAllCoroutines();
        plugin.EndVr ();
        RenderTexture.active = null;
        GL.Clear(false, true, Color.black);
    }

    IEnumerator OnResume()
	{
		Debug.LogError ("### OnResume enter 0");
        yield return StartCoroutine(plugin.BeginVr());
		Debug.LogError ("### OnResume enter 1");
        // Do this AFTER BeginVr() because it sets performance levels to it's own defaults
        plugin.SetPerformanceLevels((int)cpuPerfLevel, (int)gpuPerfLevel);

		StartCoroutine (SubmitFrame ());
        isPaused = false;
        yield break;
	}

    void LateUpdate()
    {
        if (!initialized || isPaused)
        {
            return;
        }

        Quaternion orientation = new Quaternion();
        Vector3 position = new Vector3();
        plugin.GetPredictedPose(ref orientation, ref position);

        if (!disableInput)
        {
            head.transform.localRotation = orientation;
            if (trackPosition)
            {
                head.transform.localPosition = position * trackPositionScale;
            }
        }

    }

	private void OnDestroy()
	{
		StopAllCoroutines ();

		foreach (SvrEye eye in eyes)
		{
			Component.Destroy (eye);
		}

		plugin.Shutdown ();
	}

    private AndroidJavaObject sdkBridge = null;
    private bool isScreenOn()
    {
		#if UNITY_EDITOR
		return true;
		#endif

        if (sdkBridge == null)
        {
            AndroidJavaObject curActivity = null;
            try
            {
                using (AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    curActivity = player.GetStatic<AndroidJavaObject>("currentActivity");
                }
            }
            catch (AndroidJavaException e)
            {
                curActivity = null;
                Debug.LogError("Exception while connecting to the Activity: " + e);
            }
            if (curActivity != null)
            {

                sdkBridge = new AndroidJavaObject("com.coocaa.vr.CCVRSDKBridge", curActivity);
                sdkBridge.Call("setCCVRSDKBridgeListener", new SDKBridgeCallBack(this));
            }
        }
        if (sdkBridge != null)
        {
            return sdkBridge.Call<bool>("isScreenOn");
        }
        return true;
    }

    private class SDKBridgeCallBack : AndroidJavaProxy
    {
        private SvrManager mManager;
        public SDKBridgeCallBack(SvrManager manager) : base("com.coocaa.vr.CCVRSDKBridgeListener")
        {
            mManager = manager;
        }

        public void onCrashed()
        {

        }

        public void onScreenOn()
        {
            if (mManager != null)
            {
				print("### SDKBridgeCallBack onScreenOn");
                if (!mManager.isBeginVR)
                {
					print("### onScreenOn enter isBeginVR");
                    mManager.StartCoroutine(mManager.OnResume());
                    mManager.isBeginVR = true;
                }
            }
        }

        public void onScreenOff()
        {
            if (mManager != null)
            {
				print("### SDKBridgeCallBack onScreenOff");
            }
        }
    }
}
