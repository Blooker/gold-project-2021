using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
// public class DEBUG_Data
// {
//     public float[] Data;
// }

public class PositionManager : MonoBehaviour
{
    [SerializeField] private CaptureArea IDArea;
    [SerializeField] private string Path;

    [SerializeField] private Light[] Lights;

    private const float ROT_NORM_COEFFICIENT = 1 / 720f;
    
    private float MaxSqrMagnitude;
    
    public float[,,] Data { get; private set; }
    public int DataCount { get; private set; }

    // private DEBUG_Data[] DEBUG_Data;
    
    // Start is called before the first frame update
    void Start()
    {
        IDArea.Generate(out _);
        IDArea.IsID = true;

        var numPos = IDArea.NumPos();
        MaxSqrMagnitude = (IDArea.GetPos(numPos-1).Value - IDArea.GetPos(0).Value).sqrMagnitude;
        
        // For each position in the IDArea, we record one lot of positional data
        // For each light, we record one lot of positional data and one lot of "light" data (direction and range of light)
        // This is why Lights.Length is doubled in the below line
        Data = new float[CaptureEnvironment.CaptureBatchSize, IDArea.NumPos() + Lights.Length * 2, 3];
        // DEBUG_Data = new DEBUG_Data[IDArea.NumPos() + Lights.Length * 2];
        
        DataCount = 0;
    }

    public void GenerateData(Transform t)
    {
        var numIDPos = IDArea.NumPos();
        for (int i = 0; i < numIDPos; i++)
        {
            var idPos = IDArea.GetPos(i).Value;

            var posData = GeneratePosData(t, idPos);

            Data[DataCount, i, 0] = posData.x;
            Data[DataCount, i, 1] = posData.y;
            Data[DataCount, i, 2] = posData.z;
            
            // DEBUG_Data[i].Data = new float[3];
            // DEBUG_Data[i].Data[0] = posData.x;
            // DEBUG_Data[i].Data[1] = posData.y;
            // DEBUG_Data[i].Data[2] = posData.z;
        }

        var numLights = Lights.Length;
        for (int i = 0; i < numLights; i++)
        {
            var l = Lights[i];

            var posData = Vector3.zero;
            if (l.type != LightType.Directional)
            {
                posData = GeneratePosData(t, l.transform.position);
            }

            var lightData = GenerateLightData(l);

            int dataIndex = i * 2 + numIDPos;
            
            Data[DataCount, dataIndex, 0] = posData.x;
            Data[DataCount, dataIndex, 1] = posData.y;
            Data[DataCount, dataIndex, 2] = posData.z;
            
            // DEBUG_Data[dataIndex].Data = new float[3];
            // DEBUG_Data[dataIndex].Data[0] = posData.x;
            // DEBUG_Data[dataIndex].Data[1] = posData.y;
            // DEBUG_Data[dataIndex].Data[2] = posData.z;
            
            Data[DataCount, dataIndex+1, 0] = lightData.x;
            Data[DataCount, dataIndex+1, 1] = lightData.y;
            Data[DataCount, dataIndex+1, 2] = lightData.z;
            
            // DEBUG_Data[dataIndex+1].Data = new float[3];
            // DEBUG_Data[dataIndex+1].Data[0] = lightData.x;
            // DEBUG_Data[dataIndex+1].Data[1] = lightData.y;
            // DEBUG_Data[dataIndex+1].Data[2] = lightData.z;
        }
        
        DataCount++;
    }

    private Vector3 GeneratePosData(Transform t, Vector3 newPos)
    {
        var rotA = t.rotation.eulerAngles;
            
        var dir = newPos - t.position;
            
        var rotB = Quaternion.LookRotation(dir, t.up).eulerAngles;

        var xAngle = rotB.x - rotA.x;
        var yAngle = rotB.y - rotA.y;
        
        return new Vector3(
            xAngle * ROT_NORM_COEFFICIENT + 0.5f,
            yAngle * ROT_NORM_COEFFICIENT + 0.5f,
            dir.sqrMagnitude / MaxSqrMagnitude
        );
    }

    private Vector3 GenerateLightData(Light l)
    {
        var t = l.transform;

        var rot = t.rotation.eulerAngles;
        
        float range = 1;
        if (l.type != LightType.Directional)
        {
            range = l.range;
            range = range * range / MaxSqrMagnitude;
        }
        
        return new Vector3(
            rot.x * ROT_NORM_COEFFICIENT + 0.5f,
            rot.y * ROT_NORM_COEFFICIENT + 0.5f,
            range
        );
    }
}
