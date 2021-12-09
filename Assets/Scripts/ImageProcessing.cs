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
        b = g / data.Length;

        // Create new instance of the color to be returned
        Color averageColor = new Color(r, g, b);

        return averageColor;

    }

    public static bool CompareColors(Color a, Color b, CalculationMethod method) {

        // Calculate accordingly the methods choosen

        switch(method) {
            case CalculationMethod.VECTORDISTANCE:
                // Create two vectors3 to calculate the distance between them
                Vector3 a = new Vector3(a.r, a.g, a.b);
                Vector3 b = new Vector3(b.r, b.g, b.b);

                // Calculate the distance between them
                float distance = Vector3.Distance(a, b);
                if(distance <= 0.4f) {
                    return true;
                }
                break;
            case CalculationMethod.MANHATTANDISTANCE:
                // Calculate the distance using Manhattan Distance Algorithm
                float distance = Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);

                if(distance <= 1f) {
                    return true;
                }
                break;
            case CalculationMethod.HAMMINGDISTANCE:
                // Calculate the distance using the Hamming Distance (probably the best Algorithm)
                float distance = (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 3;

                if(distance <= 0.4f) {
                    return true;
                }
                break;
            default:
                return false;
                break;
        }
        return false;
    }

}