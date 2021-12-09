using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageRecognition : MonoBehaviour {

    private ARTrackedImageManager aRTrackedImageManager;

    private void Awake() {
        aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    private void OnEnable() {
        aRTrackedImageManager.trackedImagesChanged += OnImageChange;
    }

    private void OnDisable() {
        aRTrackedImageManager.trackedImagesChanged -= OnImageChange;
    }

    public void OnImageChange(ARTrackedImagesChangedEventArgs args) {
        foreach(var trackedImage in args.added) {
            Debug.Log(trackedImage.name);
        }
    }
}
