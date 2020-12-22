using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureAreas : MonoBehaviour
{
    [SerializeField] private CaptureArea[] Areas;
    private int AreaIndex;

    public void GenerateAll()
    {
        for (int i = 0; i < Areas.Length; i++)
        {
            Areas[i].Generate();
        }
    }
    
    public Vector3? NextCamPos()
    {
        Vector3? pos;
        while (true)
        {
            if (AreaIndex >= Areas.Length)
            {
                return null;
            }
            
            pos = Areas[AreaIndex].Next();
            
            if (pos.HasValue)
            {
                break;
            }
            
            AreaIndex++;
        }

        return pos;
    }
}
