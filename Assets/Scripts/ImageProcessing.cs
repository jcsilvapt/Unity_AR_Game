using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageProcessing {

    public static Color ProcessImage(Texture2D tex) {

        // Get the color data from the center with a block size of 100*100 (total: 10000 Pixels)
        Color[] data = tex.GetPixels(tex.width / 2, tex.height / 2, 100, 100);

        // Initialize the R,G,B Variables
        float r = 0f, g = 0f, b = 0f;

        // Get the sum R,G,B of the block
        foreach(Color c in data) {
            r += c.r;
            g += c.g;
            b += c.b;
        }

        // Set the R,G,B to the average value of the color
        r = r / data.Length;
        g = g / data.Length;
        b = b / data.Length;

        // Create new instance of the color to be returned
        Color averageColor = new Color(r, g, b);

        return averageColor;

    }

    public static float GetDistance(Color a, Color b, CalculationMethod method) {

        float distance = 0;

        // Calculate accordingly the methods choosen
        switch(method) {
            case CalculationMethod.VECTORDISTANCE:
                // Create two vectors3 to calculate the distance between them
                Vector3 colorA = new Vector3(a.r, a.g, a.b);
                Vector3 colorB = new Vector3(b.r, b.g, b.b);

                // Calculate the distance between them
                return distance = Vector3.Distance(colorA, colorB);

            case CalculationMethod.MANHATTANDISTANCE:
                // Calculate the distance using Manhattan Distance Algorithm
                return distance = Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b);
            case CalculationMethod.HAMMINGDISTANCE:
                // Calculate the distance using the Hamming Distance (probably the best Algorithm)
                return distance = (Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b)) / 3;
            default:
                return 0f;
        }
    }

}