using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CaptureExport : MonoBehaviour
{
    [SerializeField] private CaptureCam Cam;

    private RenderTexture[] CaptureTextures;
    private int RowIndex;

    private int TextureCounter = 0;
    private int FileCounter = 0;

    private void Awake()
    {
        CaptureTextures = new RenderTexture[400];
    }

    public void SaveImage()
    {
        CaptureTextures[TextureCounter++] = Cam.Render();
    }

    public void NextRow()
    {
        RowIndex++;
    }
    
    public void Export()
    {
        var fileCounterOld = FileCounter;
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        for (int i = 0; i < CaptureTextures.Length; i++)
        {
            var captureTexture = CaptureTextures[i];
            
            Texture2D image = new Texture2D(captureTexture.width, captureTexture.height);
            image.ReadPixels(new Rect(0, 0, captureTexture.width, captureTexture.height), 0, 0);
            image.Apply();
 
            var bytes = image.EncodeToPNG();
            Destroy(image);
            
            File.WriteAllBytes(Application.dataPath + "/TestImages/" + FileCounter + ".png", bytes);
            FileCounter++;
        }
        stopwatch.Stop();
        
        Debug.Log($"Time to write {FileCounter - fileCounterOld} images: {stopwatch.ElapsedMilliseconds}ms");

        TextureCounter = 0;
        RowIndex = 0;
    }
}
