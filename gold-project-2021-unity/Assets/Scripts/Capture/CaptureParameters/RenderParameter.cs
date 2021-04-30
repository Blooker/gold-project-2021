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

public class RenderParameter : CaptureParameter
{
    public override float[] Output => null;
    
    [SerializeField] private MeshRenderer[] Renderers;
    [SerializeField] private MaterialPair[] MaterialPairs;
    
    private bool IsLit = true;

    private Dictionary<string, MaterialPair[]> MaterialMappings;
    
    private void Awake()
    {
        MaterialMappings = new Dictionary<string, MaterialPair[]>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        for (int i = 0; i < Renderers.Length; i++)
        {
            var materials = Renderers[i].sharedMaterials;
            var pairs = new MaterialPair[materials.Length];

            bool pairFound = false;
            for (int j = 0; j < materials.Length; j++)
            {
                var mat = materials[j];

                for (int k = 0; k < MaterialPairs.Length; k++)
                {
                    if (MaterialPairs[k].LitMaterial == mat)
                    {
                        pairs[j] = MaterialPairs[k];
                        pairFound = true;
                        break;
                    }
                }
            }
            
            if (pairFound)
            {
                MaterialMappings.Add(Renderers[i].name, pairs);
            }
        }
        
        base.Start();
    }

    protected override void UpdateParameter()
    {
        IsLit = Convert.ToBoolean(State);
        // print($"RenderManager {State}");

        for (int i = 0; i < Renderers.Length; i++)
        {
            var sharedMaterials = Renderers[i].sharedMaterials;
            if (MaterialMappings.TryGetValue(Renderers[i].name, out var pairs))
            {
                for (int j = 0; j < pairs.Length; j++)
                {
                    var pair = pairs[j];
                    sharedMaterials[j] = IsLit ? pair.LitMaterial : pair.UnlitMaterial;
                    Debug.Log($"Material set {IsLit}");
                }
            }

            Renderers[i].sharedMaterials = sharedMaterials;
        }
    }

    public bool AreRenderersVisible()
    {
        for (int i = 0; i < Renderers.Length; i++)
        {
            if (Renderers[i].isVisible)
            {
                return true;
            }
        }

        return false;
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

    protected override int MaxState()
    {
        return 2;
    }
}
