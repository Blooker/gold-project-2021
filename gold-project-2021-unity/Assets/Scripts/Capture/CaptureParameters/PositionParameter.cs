using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionParameter : CaptureParameter
{
    public override float[] OutputData { get; } = new float[3];

    public override int MaxState => NumPos;
    
    [SerializeField] private CaptureArea[] Areas;

    private int NumPos;

    private Vector3 MinPos = Vector3.positiveInfinity;
    private Vector3 MaxPos = Vector3.zero;
    
    private int AreaIndex = 0;
    private int StatePosOffset = 0;
    
    private void Start()
    {
        GeneratePositions();
    }
    
    protected override void UpdateParameter()
    {
        if (State == 0)
        {
            AreaIndex = 0;
            StatePosOffset = 0;
        }

        Vector3? pos = null;
        while (true)
        {
            int posIndex = State - StatePosOffset;
            pos = Areas[AreaIndex].GetPos(posIndex);

            if (!pos.HasValue)
            {
                StatePosOffset += Areas[AreaIndex].Length();
                AreaIndex++;

                continue;
            }

            break;
        }
        
        SetPosition(pos.Value);
    }

    
    private void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        
        for (int i = 0; i < 3; i++)
        {
            OutputData[i] = (pos[i] - MinPos[i]) / (MaxPos[i] - MinPos[i]);
        }
    }
    
    private void GeneratePositions()
    {
        // Generate all CaptureAreas
        for (int i = 0; i < Areas.Length; i++)
        {
            Areas[i].Generate(out int numPos);
            
            var localMinPos = Areas[i].GetPos(0);
            var localMaxPos = Areas[i].GetPos(numPos-1);
            
            for (int j = 0; j < 3; j++)
            {
                MinPos[j] = Mathf.Min(MinPos[j], localMinPos.Value[j]);
                MaxPos[j] = Mathf.Max(MaxPos[j], localMaxPos.Value[j]);
            }
            
            // Update total number of positions in all areas
            NumPos += numPos;
        }
    }
}