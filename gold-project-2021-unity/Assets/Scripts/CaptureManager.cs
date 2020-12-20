using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CaptureManager : MonoBehaviour
{
    [SerializeField] private Camera Cam;

    [SerializeField] private CaptureArea[] CaptureAreas;
    private int CaptureArea;

    [Header("Input")]
    [SerializeField] private KeyCode ResetKey;
    [SerializeField] private KeyCode NextCamPosKey;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ResetKey))
        {
            Reset();
        }
        
        if (Input.GetKeyDown(NextCamPosKey))
        {
            NextCamPos();
        }
    }

    public void Reset()
    {
        for (int i = 0; i < CaptureAreas.Length; i++)
        {
            CaptureAreas[i].Generate();
        }

        CaptureArea = 0;
    }
    
    public void NextCamPos()
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
