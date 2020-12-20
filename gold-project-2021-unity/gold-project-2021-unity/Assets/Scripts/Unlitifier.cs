using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Unlitifier : MonoBehaviour
{
    private static Shader Lit;
    private static Shader Unlit;
    
    [FormerlySerializedAs("materials")] [SerializeField] private Material[] Materials;

    private bool IsLit = true;
    
    private void Awake()
    {
        Lit = Shader.Find("HDRP/Lit");
        Unlit = Shader.Find("HDRP/Unlit");
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Materials.Length; i++)
        {
            Materials[i].shader = Lit;
            var litMaterial = new Material(Materials[i]);
            
            Materials[i].SetColor("_UnlitColor", litMaterial.color);
            Materials[i].SetTexture("_UnlitColorMap",  litMaterial.mainTexture);
            Materials[i].SetTextureScale("_UnlitColorMap", litMaterial.mainTextureScale);
            Materials[i].SetTextureOffset("_UnlitColorMap", litMaterial.mainTextureOffset);
        }
    }

    void ToggleLit()
    {
        IsLit = !IsLit;
        for (int i = 0; i < Materials.Length; i++)
        {
            Materials[i].shader = IsLit ? Lit : Unlit;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleLit();
        }
    }
}
