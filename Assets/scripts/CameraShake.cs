using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public struct Shake 
    {
        public float timer;
        public float magnitude;
        public float frequency;
    }

    List<Shake> shakes;

    void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("there is more than 1 instance of CameraShake!");
        }

        instance = this;

        shakes = new List<Shake>();
    }

    void LateUpdate()
    {
        
    }

    private float GetShakeAtYOffset()
    {
        return 0;
    }

    public void AddShake(float duration, float magnitude, float frequency)
    {
        Shake toAdd = new Shake();
        toAdd.timer = duration;
        toAdd.magnitude = magnitude;
        toAdd.frequency = frequency;
        shakes.Add(toAdd);
    }
}
