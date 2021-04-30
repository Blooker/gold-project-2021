using System;
using UnityEngine;

public abstract class CaptureParameter : MonoBehaviour
{
    protected int State { get; private set; } = 0;

    protected virtual void Start()
    {
        UpdateParameter();
    }

    public void Next(out bool looped)
    {
        // A function that is called to progress this capture parameter to its next state
        // Returns true if State has looped round to the beginning
            
        State++;

        looped = State >= MaxState();
        if (looped)
        {
            State = 0;
        }
        
        UpdateParameter();
    }

    public abstract float[] Output { get; }

    protected abstract void UpdateParameter();

    protected abstract int MaxState();
}
