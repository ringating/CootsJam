using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    float timer;

    public static HitStop instance;

    public bool stopped { get { return Time.timeScale != 1; } }

    [HideInInspector]
    public bool paused;

	private void Awake()
	{
		if (instance)
        {
            Debug.LogWarning("there's more than 1 instance of HitStop!");
        }

        instance = this;
	}

	void Update()
    {
        if (paused)
        {
            Time.timeScale = 0;
        }
        else 
        {
            if (timer > 0)
            {
                Time.timeScale = 0.1f;
                timer -= Time.unscaledDeltaTime;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void ApplyHitStop(float duration)
    {
        timer = duration;
    }

    public void CancelHitStop()
    {
        timer = 0;
        Time.timeScale = 1;
    }
}
