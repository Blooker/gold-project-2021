using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

class FileSplitInfo
{
    public int FileCounter;
    public LinkedList<float[]> ParameterOutputData;

    public FileSplitInfo()
    {
        ParameterOutputData = new LinkedList<float[]>();
    }
}

[SuppressMessage("ReSharper", "MergeConditionalExpression")]
public class CaptureExport : MonoBehaviour
{
    [SerializeField] private bool Export = true;
    
    [SerializeField] private string RootPath;
    
    [SerializeField] private CaptureParameter ImageSplitParameter;
    
    private FileSplitInfo[] FileSplitInfo;

    private void Awake()
    {
        FileSplitInfo = new FileSplitInfo[GetNumSplits()];
        for (int i = 0; i < FileSplitInfo.Length; i++)
        {
            FileSplitInfo[i] = new FileSplitInfo();
        }
    }

    private void Start()
    {
        CreateFolders();
    }

    public void ExportImage(Texture2D image)
    {
        if (!Export)
        {
            return;
        }
        

        var bytes = image.EncodeToPNG();
        Destroy(image);

        int state = GetCurrentSplit();
        
        File.WriteAllBytes($"{RootPath}/{state}/{FileSplitInfo[state].FileCounter++}.png", bytes);
    }

    public void AddParameterOutput(float[] parameterOutputs)
    {
        var splitInfo = FileSplitInfo[GetCurrentSplit()];
        var newNode = splitInfo.ParameterOutputData.AddLast(new float[parameterOutputs.Length]);

        for (int i = 0; i < newNode.Value.Length; i++)
        {
            newNode.Value[i] = parameterOutputs[i];
        }
    }
    
    public void ExportParameterOutputs()
    {
        for (int i = 0; i < FileSplitInfo.Length; i++)
        {
            ExportSplitParameterOutputs(i);
        }
    }

    private void ExportSplitParameterOutputs(int splitIndex)
    {
        var splitInfo = FileSplitInfo[splitIndex];
        var currentNode = splitInfo.ParameterOutputData.First;
        int parameterCount = currentNode.Value.Length;
        
        try
        {
            string path = $"{RootPath}/{splitIndex}.csv";
            using (StreamWriter file = new StreamWriter(path, false))
            {
                while (currentNode != null)
                {
                    var record = "";
                    for (int j = 0; j < parameterCount; j++)
                    {
                        record += $"{currentNode.Value[j]}";

                        if (j < parameterCount - 1)
                        {
                            record += ",";
                        }
                    }
                    
                    file.WriteLine(record);

                    currentNode = currentNode.Next;
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("WHOOPS: ", e);
        }
    }

    private void CreateFolders()
    {
        Directory.CreateDirectory(RootPath);
        
        for (int i = 0; i < FileSplitInfo.Length; i++)
        {
            Directory.CreateDirectory($"{RootPath}/{i}");
        }
    }

    private int GetCurrentSplit()
    {
        return ImageSplitParameter is null ? 0 : ImageSplitParameter.State;
    }

    private int GetNumSplits()
    {
        return ImageSplitParameter is null ? 1 : ImageSplitParameter.MaxState;
    }
}
