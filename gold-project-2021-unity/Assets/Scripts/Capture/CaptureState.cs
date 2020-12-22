using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureState
{
    public CaptureState()
    {
        Capture = 1;
        AreaStep = 1;
        
        CamPosStep = 1;
        CamRotStepX = 1;
        CamRotStepY = 1;

        Lit = true;
    }
    
    public CaptureState(CaptureState state)
    {
        Capture = state.Capture;
        AreaStep = state.AreaStep;
        
        CamPosStep = state.CamPosStep;
        CamRotStepX = state.CamRotStepX;
        CamRotStepY = state.CamRotStepY;

        Lit = state.Lit;
    }

    public int Capture;
    
    public int AreaStep;
    public int CamPosStep;
    
    public int CamRotStepX;
    public int CamRotStepY;

    public bool Lit;
}
