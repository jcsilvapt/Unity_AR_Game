using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeWebCam : MonoBehaviour {

    int currentCamIndex = 0;

    WebCamTexture tex;

    public RawImage display;

    private void Start() {
        WebCamDevice device = WebCamTexture.devices[currentCamIndex];
        tex = new WebCamTexture(device.name);

        display.texture = tex;

        tex.Play();
    }

}
