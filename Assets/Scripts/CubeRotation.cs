using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeRotation : MonoBehaviour {

    [Header("Cube Settings")]
    [Range(5f, 200f)]
    [SerializeField] float rotationSpeed = 100f;

    [Header("Face Materials")]
    [SerializeField] List<GameObject> originalCubeFaces = new List<GameObject>();

    [Header("Developer Settings")]
    [SerializeField] List<Color> originalCubeColors = new List<Color>();

    [SerializeField] bool isActive = false;
    [SerializeField] int indexPainted = 0;

    private void Start() {

        // Copiar as faces do cubo e recolher cores iniciais
        foreach (GameObject o in originalCubeFaces) {
            Color colorObject = o.GetComponent<Renderer>().material.GetColor("_Color");
            originalCubeColors.Add(colorObject);
        }

        FindObjectOfType<CharacterAnimation>().CubeFound();
        GameController.SetCube(this);
        isActive = true;
    }

    /// <summary>
    /// Method that Resets the cube to its original State
    /// </summary>
    public void ResetCube() {

        for (int i = 0; i < originalCubeColors.Count; i++) {
            originalCubeFaces[i].GetComponent<Renderer>().material.SetColor("_Color", originalCubeColors[i]);
        }
        indexPainted = 0;
    }

    private void Update() {
        if (isActive) {
            xRotation();
            yRotation();
            zRotation();
        }
    }

    private void xRotation() {

        Vector3 rotation = Vector3.up * rotationSpeed * Time.deltaTime;

        transform.eulerAngles += rotation;

    }

    private void yRotation() {

        Vector3 rotation = Vector3.right * rotationSpeed * Time.deltaTime;

        transform.eulerAngles += rotation;

    }
    private void zRotation() {

        Vector3 rotation = Vector3.forward * rotationSpeed * Time.deltaTime;

        transform.eulerAngles += rotation;

    }

    /// <summary>
    /// Method that Paints the rube
    /// </summary>
    /// <param name="color">Color to be painted in one of the cube face</param>
    /// <returns>True - Cube can/has been painted, False - Cube can't be painted</returns>
    public bool RandomPaint(Color color) {

        if (indexPainted < originalCubeFaces.Count) {
            originalCubeFaces[indexPainted].GetComponent<Renderer>().material.SetColor("_Color", color);
            indexPainted++;
            return true;
        } else {
            return false;
        }
    }

    public int GetFacesPainted() {
        return indexPainted;
    }

    public void EnableCube() {
        isActive = true;
    }

}
