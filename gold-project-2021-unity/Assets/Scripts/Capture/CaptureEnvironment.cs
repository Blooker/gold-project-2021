using System;
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

    public static int CaptureBatchSize { get; private set; }

    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        int totalPoints = 0;
        
        // Generate all CaptureAreas
        for (int i = 0; i < Areas.Length; i++)
        {
            Areas[i].Generate(out int numPoints);
            totalPoints += numPoints;
        }

        CaptureBatchSize = totalPoints * CamRotStepsX * CamRotStepsY;
        Debug.Log(CaptureBatchSize);
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
