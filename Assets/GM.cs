using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GM : MonoBehaviour {

    [Header("References")]
    [SerializeField] ARCameraManager m_CameraManager;
    [SerializeField] AROcclusionManager m_OcclusionManager;

    [Header("UI")]
    [SerializeField] RawImage m_RawCameraImage;
    [SerializeField] Text imageInfo;
    [SerializeField] Text lenghtText;

    [Header("Developer")]
    [SerializeField] Texture2D m_CameraTexture;
    [SerializeField] bool isRunning = false;

    private void OnEnable() {
        if (m_CameraManager != null) {
            m_CameraManager.frameReceived += OnCameraFrameRecieved;
        }
    }

    private void OnDisable() {
        if (m_CameraManager != null) {
            m_CameraManager.frameReceived -= OnCameraFrameRecieved;
        }
    }

    private void OnCameraFrameRecieved(ARCameraFrameEventArgs eventArgs) {
        UpdateCameraImage();
    }

    unsafe void UpdateCameraImage() {

        if (!m_CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)) {
            return;
        }

        var format = TextureFormat.RGBA32;

        if(m_CameraTexture == null || m_CameraTexture.width != image.width || m_CameraTexture.height != image.height) {
            m_CameraTexture = new Texture2D(image.width, image.height, format, false);
        }

        var conversionParams = new XRCpuImage.ConversionParams(image, format, XRCpuImage.Transformation.MirrorY);

        var rawTextureData = m_CameraTexture.GetRawTextureData<byte>();
        try {
            image.Convert(conversionParams, new System.IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
        } finally {
            image.Dispose();
        }

        m_CameraTexture.Apply();
        m_RawCameraImage.texture = m_CameraTexture;
        if(isRunning) {
            GetPixels(m_CameraTexture);
        }
    }

    private void GetPixels(Texture2D texture2d) {

        Color[] data = texture2d.GetPixels(texture2d.width / 2, texture2d.height /2, 100, 100);

        lenghtText.text = data.Length.ToString();

        float r = 0f, g = 0f, b = 0f;

        foreach(Color c in data) {
            r += c.r;
            g += c.g;
            b += c.b;
        }

        Color middlePixel = new Color((r / data.Length), (g / data.Length), (b / data.Length));

        imageInfo.text = middlePixel.ToString();

    }



    /*
    [Header("UI")]
    [SerializeField] Text isPlaying;
    [SerializeField] Text frameRate;
    [SerializeField] Text colorData;


    [Header("Android Settings")]
    [SerializeField] int currentCamIndex = 0;
    [SerializeField] WebCamDevice device;
    [SerializeField] WebCamTexture webCamTexture;
    [SerializeField] bool isDeviceRunning = false;

    [Header("Color Settings")]
    [SerializeField] Texture texture;
    [SerializeField] Vector3 selectedColorVector;
    [SerializeField] Vector3 currentColorVector;
    [SerializeField] float r = 0;
    [SerializeField] float g = 0;
    [SerializeField] float b = 0;

    [Header("Timer Settings")]
    [SerializeField] bool enableTimer = false;
    [SerializeField] float timeToProcess = 2f;
    [SerializeField] float timeElapsedOnProcess = 0f;

    [Header("Developer Settings")]
    [SerializeField] bool isRunning = false;
    [SerializeField] bool clearData = false;
    [SerializeField] int frameCounter = 0;

    private Camera cam;
    private ARCameraManager ar_cameraManager;
    private ARCameraBackground ar_cameraBackground;

    private Texture2D texture2D;
    public RenderTexture rT;
 

    private void Start() { 

        // Register the Device
        RegisterDevice();

        isRunning = true;
    }

    
    private void RegisterDevice() {
        ar_cameraManager = GetComponent<ARCameraManager>();
        ar_cameraBackground = GetComponent<ARCameraBackground>();
        device = WebCamTexture.devices[currentCamIndex];
        webCamTexture = new WebCamTexture(device.name);
        isPlaying.text = "Is Playing: " + webCamTexture.isPlaying;
    }

    private void Update() {

        Graphics.Blit(null, rT, ar_cameraBackground.material);

        if (isRunning) {
            if(!isDeviceRunning) {
                webCamTexture.Play();
                isDeviceRunning = true;
            }

            if(timeElapsedOnProcess >= timeToProcess) {
                timeElapsedOnProcess = 0f;
                DisplayInformation();
            } else {
                timeElapsedOnProcess += Time.deltaTime;
            }

        }
    }

    private void DisplayInformation() {

        frameCounter++;
        isPlaying.text = "Is Playing: " + webCamTexture.isPlaying;
        frameRate.text = "Frame Counter: " + frameCounter.ToString();
        if (webCamTexture.didUpdateThisFrame)
            colorData.text = webCamTexture.GetPixel(0, 0).ToString() + " : " + frameCounter.ToString();
        else
            colorData.text = " : " + frameCounter.ToString();
    }*/
}