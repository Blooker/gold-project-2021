using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class MaterialPair
{
    public Material LitMaterial;
    public Material UnlitMaterial;
}

public class LightingManager : MonoBehaviour
{
    private static Shader Lit;
    private static Shader Unlit;
    
    [SerializeField] private MeshRenderer[] Renderers;
    [SerializeField] private MaterialPair[] MaterialPairs;
    
    private bool IsLit = true;
    
    private void Awake()
    {
        Lit = Shader.Find("HDRP/Lit");
        Unlit = Shader.Find("HDRP/Unlit");
    }

    // Start is called before the first frame update
    void Start()
    {
        // for (int i = 0; i < Materials.Length; i++)
        // {
        //     Materials[i].shader = Lit;
        //     var litMaterial = new Material(Materials[i]);
        //
        //     Materials[i].SetColor("_UnlitColor", litMaterial.color);
        //     Materials[i].SetTexture("_UnlitColorMap",  litMaterial.mainTexture);
        //     Materials[i].SetTextureScale("_UnlitColorMap", litMaterial.mainTextureScale);
        //     Materials[i].SetTextureOffset("_UnlitColorMap", litMaterial.mainTextureOffset);
        // }
    }
    

    public void SetMaterials(Material[] mats, bool isLit) 
    {
        var newPairs = new MaterialPair[mats.Length];

        for (int i = 0; i < newPairs.Length; i++)
        {
            newPairs[i] = new MaterialPair();

            MaterialPair currentPair = null;
            
            if (i < MaterialPairs.Length)
            {
                currentPair = MaterialPairs[i];
            }
            
            newPairs[i].LitMaterial = isLit ? mats[i] : currentPair?.LitMaterial;
            newPairs[i].UnlitMaterial = isLit ?  currentPair?.UnlitMaterial : mats[i];
        }

        MaterialPairs = newPairs;
    }
    
    public void SetLit(bool isLit)
    {
        if (IsLit == isLit)
        {
            return;
        }
        
        IsLit = isLit;
        // for (int i = 0; i < Materials.Length; i++)
        // {
        //     Materials[i].shader = IsLit ? Lit : Unlit;
        // }
    }
}
