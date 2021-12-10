using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class GameController : MonoBehaviour {

    public static GameController ins;

    [Header("References")]
    [SerializeField] CameraController camC;
    [SerializeField] CubeRotation cube;

    [Header("Game Status")]
    [SerializeField] Phase currentPhase = Phase.INTRO;

    [Header("Game Settings")]
    [SerializeField] float maxGameTime = 120f;
    [SerializeField] float maxColorTime = 20f;
    [SerializeField] float elapsedMaxGameTimer = 0f;
    [SerializeField] float elapsedMaxColorTimer = 0f;
    [SerializeField] bool startRound = false;
    [SerializeField] Color generatedColor;
    [SerializeField] bool colorTimer = false;

    [Header("Android Input")]
    [SerializeField] bool hasTouchToStart = false;
    [SerializeField] bool previousTouch = false;

    [Header("Developer Settings")]
    [SerializeField] bool isRunning = false;
    [SerializeField] bool readCamera = false;


    private void Awake() {
        if (ins == null) {
            ins = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (isRunning) {
            if (!startRound) {
                if (!hasTouchToStart) {
                    bool isTouching = Input.touchCount > 0;
                    if (isTouching && !previousTouch) {
                        hasTouchToStart = true;
                        colorTimer = true;
                        StartCoroutine(StartInitialCounter());
                    }
                    previousTouch = isTouching;
                }
            } else {
                if (elapsedMaxGameTimer <= 0) {
                    GameEnded(false);
                    UIController.SetTimerText(elapsedMaxGameTimer.ToString("F2"));
                } else {
                    elapsedMaxGameTimer -= Time.deltaTime;
                }
                if (colorTimer) {
                    if (elapsedMaxColorTimer <= 0) {
                        GenerateColor();
                        elapsedMaxColorTimer = maxColorTime;
                    } else {
                        UIController.SetSubtitleText("Tens " + elapsedMaxColorTimer.ToString("F2") + " s, para encontrares a cor: ");
                        elapsedMaxColorTimer -= Time.deltaTime;
                    }
                }
                if(readCamera) {
                    GetCameraImage();
                }

            }
        }
        if(Input.GetKey(KeyCode.Escape)) {
            UIController.ShowExitPanel();
            PauseGame(true);
        }
    }

    private void GameEnded(bool win) {
        if(win) {
            UIController.SetCounterText("Ganhas-te!");
            StartCoroutine(RestartGame());

        }
    }

    private void GetCameraImage() {

        float distance = camC.GetDistance(generatedColor);
        UIController.SetDebugText("D: " + distance.ToString());

        if (distance < 0.4f) {
            StartCoroutine(DisplayMatch());
        }

    }

    private void SetInitialStatus() {
        UIController.SetSubtitleText("Clica no ecr� para come�ar!");
        UIController.SetCounterText("");
        hasTouchToStart = false;
        startRound = false;
        elapsedMaxGameTimer = maxGameTime;
        elapsedMaxColorTimer = maxColorTime;
        cube.ResetCube();
        camC.SetStatus(true);
    }

    public void GenerateColor() {
        if (!UIController.IsImageDisplaying()) {
            UIController.ShowImage();
        }

        int randomIndex = Random.Range(0, 4);
        Color randomColor = new Color();
        switch (randomIndex) {
            case 0:
                randomColor = new Color(Random.Range(0, 255), 0, 0);
                break;
            case 1:
                randomColor = new Color(0, Random.Range(0, 255), 0);
                break;
            case 2:
                randomColor = new Color(0, 0, Random.Range(0, 255));
                break;
            case 3:
                randomColor = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
                break;
        }


        UIController.SetImageColor(randomColor);

        generatedColor = randomColor;
    }

    private void GamePhase(Phase phase) {
        if (phase == Phase.GAME) {
            isRunning = true;
            SetInitialStatus();
        } else {
            isRunning = false;
        }
        currentPhase = phase;
    }

    private void PauseGame(bool value) {
        isRunning = !value;
        Time.timeScale = isRunning ? 1 : 0;
    }

    #region SINGLETON

    public static Phase GetGamePhase() {
        if (ins != null) {
            return ins.currentPhase;
        }
        return Phase.INTRO;
    }

    public static void SetGamePhase(Phase phase) {
        if (ins != null) {
            ins.GamePhase(phase);
        }
    }

    public static bool IsGameRunning() {
        if (ins != null) {
            return ins.isRunning;
        }
        return false;
    }

    public static void SetIsGameRunning(bool value) {
        if (ins != null) {
            ins.isRunning = value;
        }
    }

    public static void SetCube(CubeRotation cube) {
        if(ins != null) {
            ins.cube = cube;
        }
    }

    public static void SetPause(bool value) {
        if(ins != null) {
            ins.PauseGame(value);
        }
    }

    #endregion

    #region COROUTINES

    private IEnumerator StartInitialCounter() {
        UIController.ShowBodyPanel();

        UIController.SetSubtitleText("Prepara-te!");

        for (int i = 3; i > 0; i--) {
            UIController.SetCounterText(i.ToString());
            yield return new WaitForSeconds(1f);
        }
        UIController.SetSubtitleText("VAI!");
        yield return new WaitForSeconds(1f);
        UIController.HideBodyPanel();
        UIController.ShowImage();
        GenerateColor();
        startRound = true;
        readCamera = true;
    }

    private IEnumerator DisplayMatch() {
        readCamera = false;
        colorTimer = false;
        UIController.SetCounterText("MATCH");
        UIController.ShowBodyPanel();
        yield return new WaitForSeconds(2f);
        UIController.HideBodyPanel();
        if (!cube.IsCubePainted()) {
            cube.RandomPaint(generatedColor);
            GenerateColor();
            elapsedMaxColorTimer = maxColorTime;
            colorTimer = true;
            readCamera = true;
        } else {
            camC.SetStatus(false);
            GameEnded(true);
        }
    }

    private IEnumerator RestartGame() {
        yield return new WaitForSeconds(3f);

        SetInitialStatus();
    }
 
    #endregion

}
