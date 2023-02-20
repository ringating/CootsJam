using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum State
    {
        free,
        target
    }

    public Transform focalPoint;

    [HideInInspector]
    public State currState;
    float pitch;
    float yaw;

    void Start()
    {
        pitch = focalPoint.localRotation.eulerAngles.x;
        yaw = focalPoint.localRotation.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        switch (currState)
        {
            case State.free:
                pitch = Mathf.Clamp(pitch - Input.GetAxisRaw("Mouse Y"), -80, 80);
                yaw += Input.GetAxisRaw("Mouse X");
                focalPoint.localRotation = Quaternion.Euler(pitch, yaw, 0);
                break;

            case State.target:
                break;

            default:
                break;
        }
        
        
    }
}
