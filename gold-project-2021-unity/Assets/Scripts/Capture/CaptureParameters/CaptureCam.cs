using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CaptureCam : MonoBehaviour
{
    [SerializeField] private Vector2 RotationResolution;
    [SerializeField] private Vector2 RangeX;
    [SerializeField] private Vector2 RangeY;
    
    private int RotationX;
    private int RotationY;

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
