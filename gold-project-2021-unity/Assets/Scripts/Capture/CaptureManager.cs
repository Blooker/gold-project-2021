using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class CaptureManager : MonoBehaviour
{
    [SerializeField] private CaptureParameters Params;

    [SerializeField] private CaptureCam Cam;
    [SerializeField] private CaptureExport Export;
    
    [Header("Input")]
    [SerializeField] private KeyCode NextCamPosKey;
    [SerializeField] private KeyCode PauseToggleKey;

    private bool Paused = true;

    private void Start()
    {
        StartCoroutine(CaptureUpdate());
    }

    private void Update()
    {
        if (Input.GetKeyDown(PauseToggleKey))
        {
            Paused = !Paused;
        }
    }

    private IEnumerator CaptureUpdate()
    {
        while (true)
        {
            if (!Paused || Input.GetKeyDown(NextCamPosKey))
            {
                yield return Next();
            }
            else
            {
                yield return null;
            }
        }
    }
    
    private IEnumerator Next()
    {
        Params.Next(out bool allLooped);
        
        if (allLooped)
        {
            Export.ExportParameterOutputs();
            
            Restart();
            yield break;
        }

        yield return Cam.Render();
        yield return Export.ExportImage(Cam.RenderImage);

        Export.AddParameterOutput(Params.CurrentOutput);
    }

    private void Restart()
    {
        Params.Restart();
        Paused = true;
    }
}
