using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CaptureManager : MonoBehaviour
{
    [SerializeField] private CaptureCam Cam;

    [SerializeField] private CaptureArea[] CaptureAreas;
    private int CaptureArea;

    [SerializeField] private bool Paused = false;
    
    [Header("Input")]
    [SerializeField] private KeyCode ResetKey;
    [SerializeField] private KeyCode NextCamPosKey;
    [SerializeField] private KeyCode PauseToggleKey;

    private bool Started = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        for (int i = 0; i < CaptureAreas.Length; i++)
        {
            CaptureAreas[i].Generate();
        }

        CaptureArea = 0;
        Started = false;
    }

    void Next()
    {
        Cam.NextRotation(out bool looped);

        if (looped || !Started)
        {
            NextCamPos();
        }

        Started = true;
    }
    
    void NextCamPos()
    {
        Vector3? pos;
        while (true)
        {
            if (CaptureArea >= CaptureAreas.Length)
            {
                return;
            }
            
            pos = CaptureAreas[CaptureArea].Next();
            
            if (pos.HasValue)
            {
                break;
            }
            
            CaptureArea++;
        }

        Cam.transform.position = pos.Value;
    }
}
