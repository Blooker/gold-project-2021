using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureArea : MonoBehaviour
{
    [SerializeField] private Vector3 GridResolution;
    
    private Vector3[] Positions;
    
    private void Awake()
    {
        Positions = new Vector3[0];
    }

    public Vector3? GetPos(int pos)
    {
        if (pos >= Positions.Length)
        {
            return null;
        }

        return Positions[pos];
    }

    public int NumPos()
    {
        return (int)GridResolution.x * (int)GridResolution.y * (int)GridResolution.z;
    }
    
    public void Generate(out int numPoints)
    {
        int numX = (int)GridResolution.x;
        int numY = (int)GridResolution.y;
        int numZ = (int)GridResolution.z;

        numPoints = numX * numY * numZ;
        Positions = new Vector3[numPoints];
        
        var spacePos = transform.position;
        var spaceScale = transform.localScale;
        
        int i = 0;
        for (int x = 0; x < numX; x++)
        {
            var xPos = GetPoint(spacePos.x, spaceScale.x, x, numX);
            
            for (int y = 0; y < numY; y++)
            {
                var yPos = GetPoint(spacePos.y, spaceScale.y, y, numY);
                
                for (int z = 0; z < numZ; z++)
                {
                    var zPos = GetPoint(spacePos.z, spaceScale.z, z, numZ);
                    Positions[i++] = new Vector3(xPos, yPos, zPos);
                }
            }
        }
    }

    float GetPoint(float pos, float scale, int point, int numPoints)
    {
        return pos + scale * (point / (float)(numPoints - 1)) - scale*0.5f;
    }
    
    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.magenta;

        Gizmos.DrawWireCube(transform.position, transform.localScale);
        
        if (!Application.isPlaying)
        {
            return;
        }

        for (int i = 0; i < Positions.Length; i++)
        {
            Gizmos.DrawSphere(Positions[i], 0.2f);
        }
    }
}
