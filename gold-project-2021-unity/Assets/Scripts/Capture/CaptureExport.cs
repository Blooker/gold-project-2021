using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CaptureExport : MonoBehaviour
{
    [SerializeField] private CaptureCam Cam;
    
    private int FileCounter = 0;
    
    public void Export()
    {
        var currentRT = RenderTexture.active;
        var captureTexture = Cam.Render();
        RenderTexture.active = captureTexture;
 
        Texture2D Image = new Texture2D(captureTexture.width, captureTexture.height);
        Image.ReadPixels(new Rect(0, 0, captureTexture.width, captureTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;
 
        var Bytes = Image.EncodeToPNG();
        Destroy(Image);
 
        File.WriteAllBytes(Application.dataPath + "/TestImages/" + FileCounter + ".png", Bytes);
        FileCounter++;
    }
}
