using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float targetCamDistanceBehindCoots = 3f;
    public float targetCamDistanceAboveCoots = 3f;
    public float targetTransitionDuration = 0.25f;

    public enum State
    {
        free,
        target
    }

    public CootsController coots;

    public Transform cam;
    public Transform camFollowPoint;

    public Transform freeCamFocalPoint;
    public Transform freeCamFollowPoint;

    public Transform targetCamFocalPoint;
    public Transform targetCamFollowPoint;
    public SmoothFollowKinematicRB targetCamFollowPointSmoothed;
    public Targetable currentTarget;

    [HideInInspector]
    public State currState;
    float pitch;
    float yaw;
    float transitionTimer;
    Vector3 prevPos;
    Quaternion prevRotation;

    void Start()
    {
        pitch = freeCamFocalPoint.localRotation.eulerAngles.x;
        yaw = freeCamFocalPoint.localRotation.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;

        currState = State.free;
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

                cam.position = Vector3.Lerp(prevPos, freeCamFollowPoint.position, transitionTimer / targetTransitionDuration); ;
                cam.rotation = Quaternion.Slerp(prevRotation, freeCamFollowPoint.rotation, transitionTimer / targetTransitionDuration);

                currentFocalPoint = freeCamFocalPoint.position;
                currentFollowPoint = freeCamFollowPoint.position;

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    GetNewTarget();
                    if (currentTarget)
                    {
                        currState = State.target;
                        targetCamFollowPoint.position = GetTargetCamPos(currentTarget.transform.position);
                        targetCamFollowPointSmoothed.SnapToTarget();

                        transitionTimer = 0;
                        prevPos = cam.position;
                        prevRotation = cam.rotation;
                    }
                }

                break;

            case State.target:

                targetCamFocalPoint.position = (coots.transform.position + currentTarget.transform.position) / 2f;
                targetCamFollowPoint.position = GetTargetCamPos(targetCamFocalPoint.position);

                cam.position = Vector3.Lerp(prevPos, targetCamFollowPointSmoothed.transform.position, transitionTimer / targetTransitionDuration);
                cam.rotation = Quaternion.Slerp(prevRotation, Quaternion.LookRotation(targetCamFocalPoint.position - cam.position), transitionTimer / targetTransitionDuration);

                currentFocalPoint = targetCamFocalPoint.position;
                currentFollowPoint = targetCamFollowPoint.position;

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    currState = State.free;
                    SetCurrentPitchAndYaw();

                    transitionTimer = 0;
                    prevPos = cam.position;
                    prevRotation = cam.rotation;
                }

                break;

            default:
                Debug.LogError("how");
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

        transitionTimer += Time.deltaTime;
    }

    private void GetNewTarget() 
    {
        Targetable[] targets = FindObjectsOfType<Targetable>();

        Targetable closest = null;
        float closestDistance = 100;
        foreach (Targetable t in targets)
        {
            if (t.enabled) // can just disable Targeable scripts to make an object no longer be considered for targeting
            {
                float distance = Vector3.Distance(coots.transform.position, t.transform.position);
                if(distance < closestDistance)
				{
                    closest = t;
				}
            }
        }

        if (closest)
        {
            currentTarget = closest;
        }
        else 
        {
            // TODO: something to indicate that targeting failed (maybe a small camera shake?)
            currState = State.free;
        }
    }

    private void SetCurrentPitchAndYaw()
    {
        yaw = Vector3.SignedAngle(Vector3.forward, cam.forward, Vector3.up);

        Vector3 horizontalCamForward = cam.forward;
        horizontalCamForward.y = 0;
        pitch = Vector3.SignedAngle(horizontalCamForward, cam.forward, cam.right);
    }

    public Vector3 GetWorldMovementVector()
    {
        Vector2 unadjustedMovement = Vector2.zero;
        unadjustedMovement += Input.GetKey(KeyCode.W) ? Vector2.up : Vector2.zero;
        unadjustedMovement += Input.GetKey(KeyCode.S) ? Vector2.down : Vector2.zero;
        unadjustedMovement += Input.GetKey(KeyCode.D) ? Vector2.right : Vector2.zero;
        unadjustedMovement += Input.GetKey(KeyCode.A) ? Vector2.left : Vector2.zero;

        if (unadjustedMovement.magnitude > 1)
        {
            unadjustedMovement.Normalize();
        }

        return ( Quaternion.Euler(0, cam.rotation.eulerAngles.y, 0) * new Vector3(unadjustedMovement.x, 0, unadjustedMovement.y) );
    }

    private Vector3 GetTargetCamPos(Vector3 posToLookAtHorizontally)
    {
        Vector3 behindAndAboveCoots = coots.transform.position - posToLookAtHorizontally;
        behindAndAboveCoots.y = 0;
        behindAndAboveCoots = (behindAndAboveCoots.normalized * targetCamDistanceBehindCoots) + (Vector3.up * targetCamDistanceAboveCoots);
        return coots.transform.position + behindAndAboveCoots;
    }
}
