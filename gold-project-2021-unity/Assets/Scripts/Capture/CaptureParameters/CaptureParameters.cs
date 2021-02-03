using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CaptureParameters : MonoBehaviour
{
    [SerializeField] private CaptureEnvironment Env;
    
    [SerializeField] private CaptureCam Cam;
    [SerializeField] private RenderManager Render;
    
    public void UpdateState(CaptureState state, out bool objectsVisible)
    {
        var area = Env.Areas[state.AreaStep - 1];

        var pos = area.GetPos(state.CamPosStep - 1);
        Cam.SetPosition(pos.Value);
        
        float xPercentRot = (state.CamRotStepX - 1) / (float)(Env.CamRotStepsX-1);
        float yPercentRot = (state.CamRotStepY - 1) / (float)(Env.CamRotStepsY-1);

        Cam.SetRotation(xPercentRot, yPercentRot);

        Render.SetLit(state.Lit);

        objectsVisible = Render.AreObjectsVisible();
    }
}
