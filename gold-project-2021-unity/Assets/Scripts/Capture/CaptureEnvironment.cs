using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CaptureEnvironment : MonoBehaviour
{
    [Header("Capture Areas")]
    public CaptureArea[] Areas;
    
    [Header("Camera")]
    public int CamRotStepsX;
    public int CamRotStepsY;

    [FormerlySerializedAs("ImageBatchSize")] [Header("Images")] public int CaptureBatchSize = 100;

    public void Generate()
    {
        // Generate all CaptureAreas
        for (int i = 0; i < Areas.Length; i++)
        {
            Areas[i].Generate();
        }
    }
    
    public Vector3? GetPos(int areaIndex, int posIndex)
    {
        if (areaIndex >= Areas.Length)
        {
            return null;
        }
        
        return Areas[areaIndex].GetPos(posIndex);
    }
}
