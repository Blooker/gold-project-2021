using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingParameter : CaptureParameter
{
    public override float[] Output => null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void UpdateParameter()
    {
        throw new System.NotImplementedException();
    }

    protected override int MaxState()
    {
        return 2;
    }
}
