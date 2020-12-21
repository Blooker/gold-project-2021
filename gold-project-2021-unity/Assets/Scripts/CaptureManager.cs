using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CaptureManager : MonoBehaviour
{
    [SerializeField] private CaptureCam Cam;
    [SerializeField] private CaptureAreas Areas;

    [SerializeField] private bool Paused = false;
    
    [Header("Input")]
    [SerializeField] private KeyCode ResetKey;
    [SerializeField] private KeyCode NextCamPosKey;
    [SerializeField] private KeyCode PauseToggleKey;

    private bool Started = false;

    private void Start()
    {
        Reset();
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
            Reset();
        }
        
        if (!Paused || Input.GetKeyDown(NextCamPosKey))
        {
            Next();
        }
    }

    public void Reset()
    {
        Areas.GenerateAll();

        Started = false;
        Paused = true;
    }

    void Next()
    {
        Cam.NextRotation(out bool looped);

        // If all of the possible cam rotations have been seen,
        // or the capture run has just started, go to the next position
        if (looped || !Started)
        {
            var pos = Areas.NextCamPos();

            if (pos.HasValue)
            {
                Cam.SetPosition(pos.Value);
            }
            else
            {
                // We've reached the end, so Reset the capture run
                Reset();
            }
        }

        Started = true;
    }
}
