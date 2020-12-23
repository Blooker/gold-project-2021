using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Camera))]
public class CaptureCam : MonoBehaviour
{
    [SerializeField] private Vector2 RotationResolution;
    [SerializeField] private Vector2 RangeX;
    [SerializeField] private Vector2 RangeY;

    private Camera Cam;
    
    private int RotationX;
    private int RotationY;

    private void Awake()
    {
        Cam = GetComponent<Camera>();
        
        var targetTexture  = new RenderTexture(1920, 1080, 32);
        targetTexture.name = "Capture";
        targetTexture.enableRandomWrite = true;
        targetTexture.Create();

        Cam.targetTexture = targetTexture;
        Cam.enabled = false;
    }

    public RenderTexture Render()
    {
        Cam.Render();
        return Cam.targetTexture;
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
}
