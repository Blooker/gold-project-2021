using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CaptureExport : MonoBehaviour
{
    [SerializeField] private CaptureCam Cam;
    
    [SerializeField] private string FolderPath;
    [SerializeField] private RawImage Output;

    [SerializeField] private Color TransparentColor;
    [SerializeField] private float TransparencyThreshold;
    
    private int FileCounter = 0;

    public void SaveImage()
    {
        var rt = Cam.Render();
        Output.texture = rt;
        StartCoroutine(SaveImageRoutine(rt));
    }

    IEnumerator SaveImageRoutine(RenderTexture captureTexture)
    {
        yield return new WaitForEndOfFrame();

        Texture2D image = new Texture2D(captureTexture.width, captureTexture.height);
        image.ReadPixels(new Rect(0, 0, captureTexture.width, captureTexture.height), 0, 0);

        bool empty = true;
        
        var pixels = image.GetPixels(0, 0, captureTexture.width, captureTexture.height);
        for (int i = 0; i < pixels.Length; i++)
        {
            if (DistanceToTransparentColor(pixels[i]) < TransparencyThreshold)
            {
                int x = i % captureTexture.width;
                int y = i / captureTexture.width;
                
                image.SetPixel(x,y,Color.clear);
            }
            else
            {
                empty = false;
            }
        }
        
        if (empty)
        {
            Destroy(image);
            yield break;
        }

        image.Apply();
 
        var bytes = image.EncodeToPNG();
        Destroy(image);
            
        File.WriteAllBytes($"{FolderPath}/{FileCounter}.png", bytes);
        FileCounter++;
    }

    float DistanceToTransparentColor(Color c)
    {
        var t = TransparentColor;
        return (c.r - t.r) * (c.r - t.r) + (c.g - t.g) * (c.g - t.g) + (c.b - t.b) * (c.b - t.b);
    }
}
