using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

[SuppressMessage("ReSharper", "MergeConditionalExpression")]
public class CaptureExport : MonoBehaviour
{
    [SerializeField] private bool Export = true;
    
    [SerializeField] private string RootPath;
    
    [SerializeField] private CaptureParameter ImageSplitParameter;
    
    [SerializeField] private string PositionsPath;
    private bool PositionsExported;
    
    
    private int[] FileCounters;

    private void Awake()
    {
        int numCounters = ImageSplitParameter is null ? 1 : ImageSplitParameter.MaxState;
        FileCounters = new int[numCounters];
    }

    private void Start()
    {
        CreateFolders();
    }

    public IEnumerator ExportImage(Texture2D image)
    {
        if (!Export)
        {
            yield break;
        }
        
        yield return new WaitForEndOfFrame();

        var bytes = image.EncodeToPNG();
        Destroy(image);

        int state = ImageSplitParameter is null ? 0 : ImageSplitParameter.State;
        
        File.WriteAllBytes($"{RootPath}/{state}/{FileCounters[state]++}.png", bytes);
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

    private void CreateFolders()
    {
        Directory.CreateDirectory(RootPath);
        
        for (int i = 0; i < FileCounters.Length; i++)
        {
            Directory.CreateDirectory($"{RootPath}/{i}");
        }
    }
}
