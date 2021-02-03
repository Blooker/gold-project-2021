using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class CaptureManager : MonoBehaviour
{
    [SerializeField] private CaptureStates States;
    [SerializeField] private CaptureEnvironment Env;
    [SerializeField] private CaptureParameters Params;
    [SerializeField] private CaptureExport Export;
    
    [SerializeField] private CaptureCam Cam;

    [SerializeField] private bool Paused = false;
    
    [Header("Input")]
    [SerializeField] private KeyCode ResetKey;
    [SerializeField] private KeyCode NextCamPosKey;
    [SerializeField] private KeyCode PauseToggleKey;

    private bool Started = false;
    private bool Finished = false;

    private void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PauseToggleKey))
        {
            Paused = !Paused;
        }
        
        if (Input.GetKeyDown(ResetKey))
        {
            Initialise();
        }
        
        if (!Paused || Input.GetKeyDown(NextCamPosKey))
        {
            Next();
        }
    }

    void Initialise()
    {
        States = new CaptureStates();
        States.SaveCheckpoint();
        
        Env.Generate();

        Started = false;
        Finished = false;
    }

    void Next()
    {
        var state = States.Current;
        
        Params.UpdateState(state, out bool objectsVisible);

        if (objectsVisible)
        {
            Export.SaveImage();
        }

        state.CamRotStepX++;
        
        if (state.CamRotStepX > Env.CamRotStepsX)
        {
            state.CamRotStepX = 1;
            state.CamRotStepY++;
        }
        
        if (state.CamRotStepY > Env.CamRotStepsY)
        {
            state.CamRotStepY = 1;
            state.CamPosStep++;
        }

        var area = Env.Areas[state.AreaStep - 1];
        if (state.CamPosStep > area.NumPos())
        {
            state.CamPosStep = 1;
            state.AreaStep++;
        }

        if (state.AreaStep > Env.Areas.Length)
        {
            Finished = true;
        }
        
        state.Capture++;
        
        // If we've reached the Capture batch size or the end of the Capture run...
        if ((state.Capture-1) % Env.CaptureBatchSize == 0 || Finished)
        {
            bool lit = state.Lit;

            if (lit)
            {
                // We still have unlit captures to take, so go back to last
                // checkpoint and set finished to false
                States.LoadCheckpoint();
                Finished = false;
            }

            States.Current.Lit = !lit;
            States.SaveCheckpoint();
        }

        Started = true;

        if (Finished)
        {
            Paused = true;
            Initialise();
        }
    }
}
