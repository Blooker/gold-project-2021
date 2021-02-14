using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureParameters : MonoBehaviour
{
    [SerializeField] private CaptureEnvironment Env;
    
    [SerializeField] private CaptureCam Cam;
    [SerializeField] private CaptureExport Export;
    
    [SerializeField] private RenderManager Render;
    [SerializeField] private PositionManager Position;
    
    public IEnumerator UpdateState(CaptureState state)
    {
        var area = Env.Areas[state.AreaStep - 1];

        var pos = area.GetPos(state.CamPosStep - 1);
        Cam.SetPosition(pos.Value);
        
        float xPercentRot = (state.CamRotStepX - 1) / (float)(Env.CamRotStepsX-1);
        float yPercentRot = (state.CamRotStepY - 1) / (float)(Env.CamRotStepsY-1);

        Cam.SetRotation(xPercentRot, yPercentRot);
        
        Render.SetLit(state.Lit);
        
        if (Render.AreRenderersVisible())
        {
            yield return Cam.Render();
            if (!Cam.IsRenderEmpty())
            {
                Export.ExportImage(Cam.RenderImage, state.Lit ? 0 : 1);
                
                if (state.Lit)
                {
                    Position.GenerateData(Cam.transform);
                }
                else
                {
                    Export.ExportPositions(Position.Data, Position.DataCount);
                }
            }
        }
    }
}
