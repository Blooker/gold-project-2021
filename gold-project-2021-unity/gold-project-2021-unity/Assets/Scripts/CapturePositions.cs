using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePositions : MonoBehaviour
{
    [SerializeField] private Vector3 GridResolution;

    private Vector3[] positions;

    private void Awake()
    {
        positions = new Vector3[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Generate();
        }
    }

    void Generate()
    {
        int numX = (int)GridResolution.x;
        int numY = (int)GridResolution.y;
        int numZ = (int)GridResolution.z;
        
        positions = new Vector3[numX * numY * numZ];

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
                    positions[i++] = new Vector3(xPos, yPos, zPos);
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

        for (int i = 0; i < positions.Length; i++)
        {
            Gizmos.DrawSphere(positions[i], 0.2f);
        }
    }
}
