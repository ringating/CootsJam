using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public struct Shake 
    {
        public float timer;
        public float duration;
        public float magnitude;
        public float frequency;
        public int index;
    }

    List<Shake> shakes;
    List<Shake> toRemove;

    int tally;

    void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("there is more than 1 instance of CameraShake!");
        }

        instance = this;

        shakes = new List<Shake>();
        toRemove = new List<Shake>();
    }

    void LateUpdate()
    {
        if (!HitStop.instance.stopped)
        {
            toRemove.Clear();

            Vector3 cumulativeShake = Vector3.zero;
            for (int i = 0; i < shakes.Count; ++i)
            {
                cumulativeShake += GetShake(shakes[i]);

                // painfully decrease the timer
                Shake s = new Shake();
                s.duration = shakes[i].duration;
                s.timer = shakes[i].timer - Time.deltaTime;
                s.magnitude = shakes[i].magnitude;
                s.frequency = shakes[i].frequency;
                s.index = shakes[i].index;
                shakes[i] = s;

                if (shakes[i].timer <= 0)
                {
                    toRemove.Add(shakes[i]);
                }
            }

            foreach (Shake s in toRemove)
            {
                shakes.Remove(s);
            }

            transform.localRotation = Quaternion.Euler(cumulativeShake.x, cumulativeShake.y, cumulativeShake.z);
        }
    }

    private Vector3 GetShake(Shake shake)
    {
        Vector3 s = new Vector3(
            GetOneShake(shake, 6 * shake.index),
            GetOneShake(shake, 6 * shake.index + 2),
            GetOneShake(shake, 6 * shake.index + 4)
        );

        return s;
    }

    private float GetOneShake(Shake shake, float yOffset)
    {
        return Mathf.Sqrt(shake.timer/shake.duration) * shake.magnitude * 2f * (Mathf.PerlinNoise(shake.frequency * Time.time, yOffset) - 0.5f);
    }

    public void AddShake(float duration, float magnitude, float frequency)
    {
        Shake toAdd = new Shake();
        toAdd.timer = duration;
        toAdd.duration = duration;
        toAdd.magnitude = magnitude;
        toAdd.frequency = frequency;
        toAdd.index = tally++;
        shakes.Add(toAdd);
    }
}
