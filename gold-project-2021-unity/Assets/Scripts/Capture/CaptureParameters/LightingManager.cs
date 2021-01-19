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
    // private static Shader Lit;
    // private static Shader Unlit;
    
    [SerializeField] private MeshRenderer[] Renderers;
    [SerializeField] private MaterialPair[] MaterialPairs;
    
    private bool IsLit = true;

    private Dictionary<Material, MaterialPair> MaterialMappings;
    
    private void Awake()
    {
        MaterialMappings = new Dictionary<Material, MaterialPair>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Renderers.Length; i++)
        {
            var materials = Renderers[i].sharedMaterials;
            
            for (int j = 0; j < materials.Length; j++)
            {
                var mat = materials[j];

                for (int k = 0; k < MaterialPairs.Length; k++)
                {
                    if (MaterialPairs[k].LitMaterial == mat)
                    {
                        try
                        {
                            MaterialMappings.Add(mat, MaterialPairs[k]);
                        }
                        catch (ArgumentException e)
                        {
                            
                        }

                        break;
                    }
                }
            }
        }
    }

    public void SetLit(bool isLit)
    {
        if (IsLit == isLit)
        {
            return;
        }
        
        IsLit = isLit;
        for (int i = 0; i < Renderers.Length; i++)
        {
            var sharedMaterials = Renderers[i].sharedMaterials;
            for (int j = 0; j < Renderers[i].sharedMaterials.Length; j++)
            {
                var mat = sharedMaterials[j];

                if (mat == null)
                {
                    continue;
                }
                
                if (MaterialMappings.TryGetValue(mat, out var pair))
                {
                    sharedMaterials[j] = IsLit ? pair.LitMaterial : pair.UnlitMaterial;
                }
            }
            Renderers[i].sharedMaterials = sharedMaterials;
        }
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
}
