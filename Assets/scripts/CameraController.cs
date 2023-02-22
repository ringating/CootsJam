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

    public Transform cam;
    public Transform camFollowPoint;

    public Transform freeCamFocalPoint;
    public Transform freeCamFollowPoint;

    public Transform targetCamFocalPoint;
    public Transform targetCamFollowPoint;
    public Targetable currentTarget;

    [HideInInspector]
    public State currState;
    float pitch;
    float yaw;

    void Start()
    {
        pitch = freeCamFocalPoint.localRotation.eulerAngles.x;
        yaw = freeCamFocalPoint.localRotation.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        Vector3 currentFocalPoint;
        Vector3 currentFollowPoint;

        switch (currState)
        {
            case State.free:

                pitch = Mathf.Clamp(pitch - Input.GetAxisRaw("Mouse Y"), -80, 80);
                yaw += Input.GetAxisRaw("Mouse X");
                freeCamFocalPoint.localRotation = Quaternion.Euler(pitch, yaw, 0);

                cam.position = freeCamFollowPoint.position;
                cam.rotation = freeCamFollowPoint.rotation;

                currentFocalPoint = freeCamFocalPoint.position;
                currentFollowPoint = freeCamFollowPoint.position;

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    currState = State.target;
                    GetNewTarget();
                }

                break;

            case State.target:
                currentFocalPoint = targetCamFocalPoint.position;
                currentFollowPoint = targetCamFollowPoint.position;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    currState = State.free;
                }
                break;

            default:
                currentFocalPoint = freeCamFocalPoint.position;
                currentFollowPoint = freeCamFollowPoint.position;
                break;
        }

        // resolve the camera towards the current focal point
        /*Physics.Raycast(
            currentFocalPoint,
            cam.position - currentFocalPoint, 
            out RaycastHit hitInfo, 
            Vector3.Distance(cam.position, currentFocalPoint), 
            LayerMask.GetMask("terrain"),
            QueryTriggerInteraction.Ignore
        );*/
    }

    private void GetNewTarget() 
    {

    }

    private void SetCurrentPitchAndYaw()
    {

    }
}
