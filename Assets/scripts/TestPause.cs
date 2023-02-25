using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPause : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            HitStop.instance.paused = !HitStop.instance.paused;
        }
    }
}
