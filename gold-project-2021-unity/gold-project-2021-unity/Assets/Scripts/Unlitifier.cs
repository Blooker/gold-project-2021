using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Unlitifier : MonoBehaviour
{
    private bool isLit = true;
    private static Shader lit;
    private static Shader unlit;
    
    [SerializeField] private Material[] materials;

    private Material[] litMaterials;
    
    private void Awake()
    {
        lit = Shader.Find("HDRP/Lit");
        unlit = Shader.Find("HDRP/Unlit");
    }

    // Start is called before the first frame update
    void Start()
    {
        litMaterials = new Material[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].shader = lit;
            litMaterials[i] = new Material(materials[i]);
            
            materials[i].SetColor("_UnlitColor", litMaterials[i].color);
            materials[i].SetTexture("_UnlitColorMap",  litMaterials[i].mainTexture);
            materials[i].SetTextureScale("_UnlitColorMap", litMaterials[i].mainTextureScale);
            materials[i].SetTextureOffset("_UnlitColorMap", litMaterials[i].mainTextureOffset);
        }
    }

    void ToggleLit()
    {
        isLit = !isLit;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].shader = isLit ? lit : unlit;
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
