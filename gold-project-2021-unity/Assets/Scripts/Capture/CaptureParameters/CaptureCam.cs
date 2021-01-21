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

    public RenderTexture Render()
    {
        return TargetTexture;
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
