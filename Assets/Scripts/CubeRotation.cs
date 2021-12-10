using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotation : MonoBehaviour {

    [Header("Cube Settings")]
    [Range(5f, 200f)]
    [SerializeField] float rotationSpeed = 100f;

    [Header("Face Materials")]
    [SerializeField] List<Material> cubeMaterials = new List<Material>();

    [Header("Developer Settings")]
    [SerializeField] List<Material> cubeMaterialsOriginal = new List<Material>();
    [SerializeField] List<bool> cubeColorChanged = new List<bool>();
    [SerializeField] bool isCubePainted = false;

    [SerializeField] bool isActive = false;

    private void Start() {

        foreach (Material m in cubeMaterials) {
            Material matCopy = new Material(m);
            cubeMaterialsOriginal.Add(matCopy);
            cubeColorChanged.Add(false);
        }

        FindObjectOfType<CharacterAnimation>().CubeFound();
        GameController.SetCube(this);
        isActive = true;
    }

    public void ResetCube() {
        for (int i = 0; i < cubeMaterials.Count; i++) {
            cubeMaterials[i] = cubeMaterialsOriginal[i];
            cubeColorChanged[i] = false;
        }
    }

    private void Update() {
        if (isActive) {
            xRotation();
            zRotation();
        }
    }

    private void xRotation() {

        Vector3 rotation = Vector3.up * rotationSpeed * Time.deltaTime;

        transform.localEulerAngles += rotation;

    }

    private void zRotation() {

        Vector3 rotation = Vector3.forward * rotationSpeed * Time.deltaTime;

        transform.localEulerAngles += rotation;

    }

    public void RandomPaint(Color color) {
        bool isPaiting = true;

        while(isPaiting) {
            int value = Random.Range(0, cubeMaterials.Count);

            if(!cubeColorChanged[value]) {
                cubeMaterials[value].color = color;
                cubeColorChanged[value] = true;
                isPaiting = false;
                return;
            }

        }
    }

    public bool IsCubePainted() {

        foreach(bool v in cubeColorChanged) {
            if(v == false) {
                return false;
            }
        }
        return true;
    }

    public void EnableCube() {
        isActive = true;
    }

}
