using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    [SerializeField] private CaptureArea IDArea;
    [SerializeField] private string Path;
    
    private float MaxSqrMagnitude;
    
    public float[,,] Data { get; private set; }
    public int DataCount { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        IDArea.Generate(out _);
        IDArea.IsID = true;

        var numPos = IDArea.NumPos();
        MaxSqrMagnitude = (IDArea.GetPos(numPos-1).Value - IDArea.GetPos(0).Value).sqrMagnitude;
        
        Data = new float[CaptureEnvironment.CaptureBatchSize, IDArea.NumPos(), 3];
        DataCount = 0;
    }

    public void GenerateData(Transform t)
    {
        var numIDPos = IDArea.NumPos();
        for (int i = 0; i < numIDPos; i++)
        {
            var idPos = IDArea.GetPos(i).Value;

            var rotA = t.rotation.eulerAngles;
            
            var dir = idPos - t.position;
            
            var rotB = Quaternion.LookRotation(dir, t.up).eulerAngles;

            var xAngle = rotB.x - rotA.x;
            var yAngle = rotB.y - rotA.y;
            
            Data[DataCount, i, 0] = xAngle / 720 + 0.5f;
            Data[DataCount, i, 1] = yAngle / 720 + 0.5f;
            Data[DataCount, i, 2] = dir.sqrMagnitude / MaxSqrMagnitude;
        }
        
        DataCount++;
    }
}
