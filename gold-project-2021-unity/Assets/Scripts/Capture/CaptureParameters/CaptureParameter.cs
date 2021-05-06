using System;
using UnityEngine;

public abstract class CaptureParameter : MonoBehaviour
{
    public int State { get; private set; } = -1;

    public void Next(out bool looped)
    {
        // A function that is called to progress this capture parameter to its next state
        // Returns true if State has looped round to the beginning
        
        State++;
        
        looped = State >= MaxState;
        if (looped)
        {
            State = 0;
        }
        
        UpdateParameter();
    }

    public void Restart()
    {
        State = -1;
    }

    public abstract float[] OutputData { get; }

    public abstract int MaxState { get; }
    
    protected abstract void UpdateParameter();
}
