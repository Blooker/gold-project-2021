using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationParameter : CaptureParameter
{
    public override float[] Output { get; } = new float[2];
    
    [SerializeField] private Vector2 RotationResolution;
    
    [SerializeField] private Vector2 RangeX;
    [SerializeField] private Vector2 RangeY;
    
    private int RotationX;
    private int RotationY;

    protected override void UpdateParameter()
    {
        var xPercent = (State % RotationResolution.x) / (RotationResolution.x - 1);
        var yPercent = (int)(State / RotationResolution.x) / (RotationResolution.y - 1);
        
        SetRotation(xPercent, yPercent);
    }

    public void SetRotation(float xPercent, float yPercent)
    {
        var rotX = Mathf.Lerp(RangeX.x, RangeX.y, xPercent);
        var rotY = Mathf.Lerp(RangeY.x, RangeY.y, yPercent);

        transform.localRotation = Quaternion.Euler(rotX, rotY, 0);

        var rangeXDiff = Mathf.Abs(RangeX.x - RangeX.y);
        Output[0] = (rotX + rangeXDiff * 0.5f) / rangeXDiff;
        
        var rangeYDiff = Mathf.Abs(RangeY.x - RangeY.y);
        Output[1] = (rotY + rangeYDiff * 0.5f) / rangeYDiff;

        Debug.Log($"{Output[0]}, {Output[1]}");
    }
    
    protected override int MaxState()
    {
        return (int)RotationResolution.x * (int)RotationResolution.y;
    }
}
