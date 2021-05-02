using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CaptureExport : MonoBehaviour
{
    [SerializeField] private bool Export = true;
    [SerializeField] private string[] ImagePaths;

    [SerializeField] private string PositionsPath;
    private bool PositionsExported;
    
    
    private int[] FileCounters;

    private void Awake()
    {
        FileCounters = new int[ImagePaths.Length];
    }

    public void ExportImage(Texture2D render, int pathIndex)
    {
        if (!Export)
        {
            return;
        }
        
        StartCoroutine(ExportImageRoutine(render, pathIndex));
    }

    IEnumerator ExportImageRoutine(Texture2D image, int pathIndex)
    {
        yield return new WaitForEndOfFrame();

        var bytes = image.EncodeToPNG();
        Destroy(image);
        
        File.WriteAllBytes($"{ImagePaths[pathIndex]}/{FileCounters[pathIndex]}.png", bytes);
        
        FileCounters[pathIndex]++;
    }

    public void ExportPositions(float[,,] data, int dataCount)
    {
        if (PositionsExported)
        {
            return;
        }
        
        try
        {
            using (StreamWriter file = new StreamWriter(PositionsPath, true))
            {
                for (int i = 0; i < dataCount; i++)
                {
                    var numPos =  data.GetLength(1);
                    var record = "";
                    for (int j = 0; j < numPos; j++)
                    {
                        record += $"{data[i, j, 0]},{data[i, j, 1]},{data[i, j, 2]}";

                        if (j < numPos - 1)
                        {
                            record += ",";
                        }
                    }
                    
                    file.WriteLine(record);
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("WHOOPS: ", e);
        }

        PositionsExported = true;
    }
}
