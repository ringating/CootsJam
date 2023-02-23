using UnityEngine;

public class LockFramerate : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;

        if (Application.isEditor)
        {
            Application.targetFrameRate = 20; // just to make sure stuff looks good at the most standard framerate
        }
        else 
        {
            Application.targetFrameRate = 999;
        }        
    }
}
