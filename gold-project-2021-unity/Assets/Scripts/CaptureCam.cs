using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureCam : MonoBehaviour
{
    [SerializeField] private Vector2 RotationResolution;
    [SerializeField] private Vector2 XLimits;
    [SerializeField] private Vector2 YLimits;
    
    private int RotationX;
    private int RotationY;

    public void NextRotation(out bool looped)
    {
        looped = false;

        if (RotationX > (int) RotationResolution.x)
        {
            RotationY++;
            if (RotationY > (int) RotationResolution.y)
            {
                RotationY = 0;
                looped = true;
            }
            
            RotationX = 0;
        }
        
        UpdateRotation(RotationX++, RotationY);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    
    void UpdateRotation(int x, int y)
    {
        var xPercent = x / RotationResolution.x;
        var yPercent = y / RotationResolution.y;
        
        var rotX = Mathf.Lerp(XLimits.x, XLimits.y, xPercent);
        var rotY = Mathf.Lerp(YLimits.x, YLimits.y, yPercent);

        transform.localRotation = Quaternion.Euler(rotX, rotY, 0);
    }
}
