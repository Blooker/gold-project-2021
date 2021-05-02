using System;
using UnityEngine;

[System.Serializable]

public class CaptureParameters : MonoBehaviour
{
    [SerializeField] private CaptureParameter[] Parameters;
    
    public float[] OutputData { get; private set; }

    private bool InitialState = true;
    
    private void Start()
    {
        int outputLength = 0;
        for (int i = 0; i < Parameters.Length; i++)
        {
            // If this parameter's output data is not null, then add its length to the total
            outputLength += Parameters[i].OutputData?.Length ?? 0;
        }
        
        OutputData = new float[outputLength];
    }

    public void Next(out bool allLooped)
    {
        allLooped = true;

        int outputIndex = 0;
        foreach (var parameter in Parameters)
        {
            parameter.Next(out bool looped);
            allLooped &= looped;
            
            if (parameter.OutputData != null)
            {
                foreach (float output in parameter.OutputData)
                {
                    OutputData[outputIndex++] = output;
                }
            }
            
            if (!(InitialState || looped))
            {
                break;
            }
        }

        InitialState = false;
    }

    public void Restart()
    {
        foreach (var parameter in Parameters)
        {
            parameter.Restart();
        }
        
        InitialState = true;
    }
}