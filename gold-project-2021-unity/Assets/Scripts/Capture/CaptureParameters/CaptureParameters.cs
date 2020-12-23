using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureParameters : MonoBehaviour
{
    [SerializeField] private CaptureEnvironment Env;
    
    [SerializeField] private CaptureCam Cam;
    [SerializeField] private LightingManager Lighting;
    
    public void UpdateState(CaptureState state)
    {
        var area = Env.Areas[state.AreaStep - 1];

        var pos = area.GetPos(state.CamPosStep - 1);
        Cam.SetPosition(pos.Value);
        
        float xPercentRot = (state.CamRotStepX - 1) / (float)(Env.CamRotStepsX-1);
        float yPercentRot = (state.CamRotStepY - 1) / (float)(Env.CamRotStepsY-1);

        Cam.SetRotation(xPercentRot, yPercentRot);

        Lighting.SetLit(state.Lit);
        
        Cam.Render();
    }
}
