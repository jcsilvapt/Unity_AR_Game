using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    [Header("References")]
    [SerializeField] Animator anim;

    [Header("Subtitles")]
    [SerializeField] List<string> initialSubtitles = new List<string>();
    [SerializeField] List<string> finalSubtitles = new List<string>();

    [Header("Developer Settings")]
    [SerializeField] bool isAnimationPlaying = true;
    [SerializeField] int textToShowIndex = 0;
    [SerializeField] string lastClipName = "";
    [SerializeField] bool isFadingOccurring = false;
    [SerializeField] bool runLogic = false;
    [SerializeField] float timePerText = 3f;
    [SerializeField] float elapsedTime = 0f;
    [SerializeField] bool finishWaving = false;
    [SerializeField] bool allowResize = true;

    private void Start() {

        anim = GetComponent<Animator>();

        if (GameController.GetGamePhase() == Phase.GAME) {
            anim.SetBool("Greetings", false);
            anim.SetBool("isIntro", false);
        } else {
            UIController.SetSubtitleText(initialSubtitles[textToShowIndex]);
        }

    }

    private void Update() {
        // If Game is running theres no need to play intro animations
        if (GameController.IsGameRunning()) return;

        if (isAnimationPlaying) {
            if (anim.GetCurrentAnimatorClipInfo(0).Length > 0) {
                string currentClip = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

                if (!currentClip.Equals(lastClipName)) {
                    lastClipName = currentClip;

                    if (lastClipName.Equals("Waving")) {
                        isAnimationPlaying = false;
                        runLogic = true;
                    }
                }
            }
        }

        if (runLogic) {
            if (textToShowIndex < initialSubtitles.Count) {
                if (elapsedTime >= timePerText) {
                    if (!finishWaving) {
                        finishWaving = true;
                        anim.SetBool("Greetings", false);
                    }
                    elapsedTime = 0f;
                    ChangeText();
                } else {
                    elapsedTime += Time.deltaTime;
                }
            } else {
                runLogic = false;
                UIController.SetCounterText("Run Logic False");
                Debug.Log("Run Logic False");
            }
        }
    }

    private void ChangeText() {
        textToShowIndex++;
        if (textToShowIndex == initialSubtitles.Count) {
            StartCoroutine(ResizeCharacter());
        }
        Debug.Log("subtitle Changed");
        UIController.SetSubtitleText(initialSubtitles[textToShowIndex - 1]);

    }

    // Animation Event
    private void CallSubtitles() {
        if (!GameController.IsGameRunning()) {
            UIController.ShowSubtitlePanel();
            isAnimationPlaying = true;
        }
    }

    public void CubeFound() {
        if (!GameController.IsGameRunning()) {
            StartCoroutine(StartGame());
        }
    }

    #region COROUTINES

    private IEnumerator StartGame() {

        foreach(string text in finalSubtitles) {
            UIController.SetSubtitleText(text);
            yield return new WaitForSeconds(4f);
        }
        GameController.SetGamePhase(Phase.GAME);
    }

    private IEnumerator ResizeCharacter() {
        if (allowResize) {
            bool isResizing = true;
            int freeCounter = 0;
            while (isResizing) {
                Vector3 currentSize = transform.localScale;
                Vector3 objectiveSize = new Vector3(0.05f, 0.05f, 0.05f);
                Vector3 currentPosition = transform.localPosition;
                Vector3 objectivePosition = new Vector3(-0.09f, -0.15f, 0.4f);


                Vector3 size = Vector3.Lerp(currentSize, objectiveSize, Time.deltaTime);
                Vector3 position = Vector3.Lerp(currentPosition, objectivePosition, Time.deltaTime);
                transform.localScale = size;
                transform.localPosition = position;
                freeCounter++;

                if ((transform.localScale.x >= 0.05f && transform.localScale.x <= 0.0505) && (transform.localPosition.x >= -0.09f && transform.localPosition.x <= -0.0899)) {
                    isResizing = false;

                }
                yield return null;
            }
        }
        yield return null;
    }

    #endregion

}
