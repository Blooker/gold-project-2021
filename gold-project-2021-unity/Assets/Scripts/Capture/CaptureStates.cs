using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureStates
{
    public CaptureStates()
    {
        Current = new CaptureState();
    }

    public CaptureState Current { get; private set; }
    private CaptureState Checkpoint;

    public void SaveCheckpoint()
    {
        Checkpoint = new CaptureState(Current);
    }

    public void LoadCheckpoint()
    {
        Current = new CaptureState(Checkpoint);
    }
}
