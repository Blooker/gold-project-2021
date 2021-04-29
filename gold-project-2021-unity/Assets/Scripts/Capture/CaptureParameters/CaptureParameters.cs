using UnityEngine;

public class CaptureParameters : MonoBehaviour
{
    [SerializeField] private CaptureParameter[] Parameters;

    public void Next(out bool allLooped)
    {
        allLooped = false;
        
        for (int i = 0; i < Parameters.Length; i++)
        {
            Parameters[i].Next(out bool looped);

            if (!looped)
            {
                return;
            }
        }

        allLooped = true;
    }
}