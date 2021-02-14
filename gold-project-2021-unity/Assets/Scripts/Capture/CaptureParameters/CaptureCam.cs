using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CaptureCam : MonoBehaviour
{
    public Texture2D RenderImage { get; private set; }

    [SerializeField] private Vector2 RotationResolution;
    [SerializeField] private Vector2 RangeX;
    [SerializeField] private Vector2 RangeY;

    [SerializeField] private RawImage Output;
    
    [SerializeField] private Color BGColor;
    [SerializeField] private float BGThreshold;
    
    private Camera Cam;

    private RenderTexture TargetTexture;
    private int RotationX;
    private int RotationY;

    private void Awake()
    {
        Cam = GetComponent<Camera>();
        
        TargetTexture  = new RenderTexture(Screen.width, Screen.height, 32);
        TargetTexture.name = "Capture";
        TargetTexture.enableRandomWrite = true;
        TargetTexture.Create();

        Cam.targetTexture = TargetTexture;
    }

    public IEnumerator Render()
    {
        Output.texture = TargetTexture;
        yield return new WaitForEndOfFrame();
        
        Destroy(RenderImage);
        RenderImage = new Texture2D(TargetTexture.width, TargetTexture.height);
        RenderImage.ReadPixels(new Rect(0, 0, TargetTexture.width, TargetTexture.height), 0, 0);
    }

    public bool IsRenderEmpty()
    {
        var pixels = RenderImage.GetPixels(0, 0, TargetTexture.width, TargetTexture.height);
        for (int i = 0; i < pixels.Length; i++)
        {
            if (DistanceToBGColor(pixels[i]) >= BGThreshold)
            {
                return false;
            }
        }

        return true;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    
    public void SetRotation(float xPercent, float yPercent)
    {
        var rotX = Mathf.Lerp(RangeX.x, RangeX.y, xPercent);
        var rotY = Mathf.Lerp(RangeY.x, RangeY.y, yPercent);

        transform.localRotation = Quaternion.Euler(rotX, rotY, 0);
    }
    
    float DistanceToBGColor(Color c)
    {
        var bg = BGColor;
        return (c.r - bg.r) * (c.r - bg.r) + (c.g - bg.g) * (c.g - bg.g) + (c.b - bg.b) * (c.b - bg.b);
    }
}
