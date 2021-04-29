using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class CaptureManager : MonoBehaviour
{
    [FormerlySerializedAs("ParamsNew")] [SerializeField] private CaptureParameters Params;
    
    [SerializeField] private bool Paused = false;
    
    [Header("Input")]
    [SerializeField] private KeyCode ResetKey;
    [SerializeField] private KeyCode NextCamPosKey;
    [SerializeField] private KeyCode PauseToggleKey;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PauseToggleKey))
        {
            Paused = !Paused;
        }

        if (!Paused || Input.GetKeyDown(NextCamPosKey))
        {
            Next();
        }
    }

    void Next()
    {
        Params.Next(out bool allLooped);
        
        if (allLooped)
        {
            Paused = true;
        }
    }
}
