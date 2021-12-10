using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CameraController : MonoBehaviour {

    [Header("References")]
    [SerializeField] ARCameraManager m_CameraManager;
    [SerializeField] AROcclusionManager m_OcclusionManager;

    [Header("Developer")]
    [SerializeField] RawImage m_RawCameraImage;
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
        if (isRunning)
            UpdateCameraImage();
    }

    unsafe void UpdateCameraImage() {

        if (!m_CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)) {
            return;
        }

        var format = TextureFormat.RGBA32;

        if (m_CameraTexture == null || m_CameraTexture.width != image.width || m_CameraTexture.height != image.height) {
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
    }

    public float GetDistance(Color generatedColor) {
        if (isRunning) {

            UIController.SetDebugPixelsLenghtText(m_CameraTexture.GetPixels().Length.ToString());

            Color cameraColor = ImageProcessing.ProcessImage(m_CameraTexture);
            

            UIController.SetDebugRGBText(cameraColor.ToString());

            // Check the distance using Hamming Distance (Better performance)
            return ImageProcessing.GetDistance(ConvertColor(generatedColor), cameraColor, CalculationMethod.VECTORDISTANCE);
        }
        return 0f;
    }

    private Color ConvertColor(Color color) {

        return new Color(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
    }

    public void SetStatus(bool value) {
        isRunning = value;
    }

}
