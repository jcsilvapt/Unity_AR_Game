using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController ins;

    [Header("References")]
    [SerializeField] CanvasGroup subtitlePanel;
    [SerializeField] CanvasGroup timerPanel;
    [SerializeField] CanvasGroup bodyPanel;
    [SerializeField] Text subtitlesText;
    [SerializeField] Text timerText;
    [SerializeField] Text counterText;
    [SerializeField] CameraController camC;
    [SerializeField] GameObject imageGO;
    [SerializeField] Image image;

    [Header("Developer Settings - UI")]
    [Tooltip("Enable this to show in the device a debug Information")]
    [SerializeField] bool showDebug = false;
    [SerializeField] GameObject debugPanel;
    [SerializeField] Text pixelsLenghtText;
    [SerializeField] Text framesText;
    [SerializeField] Text rgbDataText;
    [SerializeField] Text debugText;
    [SerializeField] bool isFading = false;

    private float timerPerFrame = 1f;
    private float elapsedTime = 0f;
    private int frameCounter = 0;

    private void Awake() {
        if (ins == null) {
            ins = this;
            DontDestroyOnLoad(this);

            SetInitialSetup();
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (elapsedTime >= timerPerFrame) {
            frameCounter++;
            elapsedTime = 0f;
            framesText.text = frameCounter.ToString();
        } else {
            elapsedTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Method that Sets all Canvas and Text to default values
    /// </summary>
    private void SetInitialSetup() {

        camC = GetComponent<CameraController>();

        if (debugPanel != null) {
            debugPanel.SetActive(showDebug ? true : false);
        }

        if (bodyPanel != null) {
            bodyPanel.alpha = 0;
        }
        if (subtitlePanel != null) {
            subtitlePanel.alpha = 0;
        }
        if (timerPanel != null) {
            timerPanel.alpha = 0;
        }

        debugText.text = "No Log";

        imageGO.SetActive(false);
    }

    private void SetDebugLog(string text) {
        debugText.text = text;
    }

    #region COROUTINES

    private IEnumerator FadeInPanel(CanvasGroup cg) {
        Debug.Log("Fade Effect on " + cg.name);

        while (cg.alpha < 1) {
            cg.alpha += Time.deltaTime * 2f;
            yield return null;
        }

        Debug.Log("Fade Effect Done on " + cg.name);

    }

    #endregion

    #region SINGLETON

    /// <summary>
    /// Method that sets the subtitle text in the UI
    /// </summary>
    /// <param name="text"></param>
    public static void SetSubtitleText(string text) {
        if (ins != null) {
            ins.subtitlesText.text = text;
        }
    }

    /// <summary>
    /// Method that sets the current game timer in the UI
    /// NOTE: Just insert the numeric value
    /// </summary>
    /// <param name="text"></param>
    public static void SetTimerText(string text) {
        if (ins != null) {
            ins.timerText.text = text + " segundos";
        }
    }

    /// <summary>
    /// Method that Set the Middle Screen Counter Text in the UI
    /// </summary>
    /// <param name="text"></param>
    public static void SetCounterText(string text) {
        if (ins != null) {
            ins.counterText.text = text;
        }
    }

    /// <summary>
    /// Method that shows the Debugs Logs
    /// </summary>
    /// <param name="text"></param>
    public static void SetDebugText(string text) {
        if (ins != null) {
            ins.SetDebugLog(text);
        }
    }

    public static void SetDebugPixelsLenghtText(string text) {
        if (ins != null) {
            ins.pixelsLenghtText.text = text;
        }
    }

    public static void SetDebugFrameCounter(string text) {
        if (ins != null) {
            ins.framesText.text = text;
        }
    }

    public static void SetDebugRGBText(string text) {
        if (ins != null) {
            ins.rgbDataText.text = text;
        }
    }


    /// <summary>
    /// Method that Shows the Subtitle Panel
    /// </summary>
    public static void ShowSubtitlePanel() {
        if (ins != null) {
            ins.StartCoroutine(ins.FadeInPanel(ins.subtitlePanel));
        }
    }


    public static void ShowTimerPanel() {
        if (ins != null) {
            ins.StartCoroutine(ins.FadeInPanel(ins.timerPanel));
        }
    }

    public static void ShowBodyPanel() {
        if (ins != null) {
            ins.bodyPanel.alpha = 1;
        }
    }

    public static void HideBodyPanel() {
        if (ins != null) {
            ins.bodyPanel.alpha = 0;
        }
    }

    public static void ShowImage() {
        if (ins != null) {
            ins.imageGO.SetActive(true);
        }
    }

    public static void HideImage() {
        if (ins != null) {
            ins.imageGO.SetActive(false);
        }
    }

    public static bool IsImageDisplaying() {
        if (ins != null) {
            return ins.imageGO.activeSelf;
        }
        return false;
    }

    public static void SetImageColor(Color c) {
        if (ins != null) {
            ins.image.color = c;
        }
    }

    #endregion

    public void ToggleDebug() {
        if (!showDebug) {
            showDebug = true;
            debugPanel.SetActive(true);
        } else {
            showDebug = false;
            debugPanel.SetActive(false);
        }
    }

}
