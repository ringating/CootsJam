using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform focalPoint;

    float pitch;
    float yaw;

    void Start()
    {
        pitch = focalPoint.localRotation.eulerAngles.x;
        yaw = focalPoint.localRotation.eulerAngles.y;
    }

    void Update()
    {
        
    }
}
