using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class SvrEye : MonoBehaviour
{
    public enum Side
    {
        Left,
        Right,
        [HideInInspector]
        Count
    };

    public delegate void OnPostRenderCallback();
    public OnPostRenderCallback OnPostRenderListener;

    public RenderTextureFormat format = RenderTextureFormat.Default;
    public Side side = Side.Left;
    public Vector2 textureSize = new Vector2(1024.0f, 1024.0f);
    public Vector2 fov = new Vector2(90.0f, 90.0f);
    public int antiAliasing = 1;
    public int depth = 24;

    private const int bufferCount = 3;
    private RenderTexture[] eyeTextures = new RenderTexture[bufferCount];
    private int[] eyeTextureIds = new int[bufferCount];
    private int currentTextureIndex = 0;
    private Camera mainCamera = null;

    public int TextureId
    {
        get { return eyeTextureIds[currentTextureIndex]; }
    }

    void Awake()
    {
        AcquireComponents();
    }

    void AcquireComponents()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        InitializeBuffers();
        InitializeCamera();
    }

    void InitializeBuffers()
    {
        for (int i = 0; i < bufferCount; ++i)
        {
            eyeTextures[i] = new RenderTexture((int)textureSize.x, (int)textureSize.y, depth, format);
            eyeTextures[i].antiAliasing = antiAliasing;
            eyeTextures[i].Create();
            eyeTextureIds[i] = eyeTextures[i].GetNativeTexturePtr().ToInt32();
        }
    }

    void InitializeCamera()
    {
        mainCamera.fieldOfView = fov.x;
        mainCamera.aspect = textureSize.x / textureSize.y;
        mainCamera.depth = (int)side;
    }

    void OnPreRender()
    {
        SwapBuffers();
    }

    void SwapBuffers()
    {
        currentTextureIndex = ++currentTextureIndex % bufferCount;
        mainCamera.targetTexture = eyeTextures[currentTextureIndex];
        mainCamera.targetTexture.DiscardContents();
    }

    void OnPostRender()
    {
        if (OnPostRenderListener != null)
        {
            OnPostRenderListener();
        }
    }
}
