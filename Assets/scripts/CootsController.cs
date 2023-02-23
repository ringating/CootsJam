using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CootsController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float turnRate = 360f;
    public float attackDuration = 0.27f;

    const float oneFrame = 1f / 60f;

    public enum State
    {
        idle,
        walk,
        attack,
        dodge,
    }

    [HideInInspector]
    public State currState;
    private State nextState;
    private bool restartState = false;
    private float targetYaw;

    //public CharacterController cc;
    public Rigidbody rb;
    public Animator animator;
    public Animator impactAnimator;
    public Slash slashScript;
    public CameraController camScript;

    void Start()
    {
        //cc = GetComponent<CharacterController>();
    }

    // ############## per-state vars ##############

    // attack
    bool attackButtonReleased = false;
    bool bufferAnotherAttack = false;
    float attackTimer = 0;
    bool attackFlipFlop;

    // dodge
    [HideInInspector]
    public bool invincible;
    [HideInInspector]
    public bool canCancelDodge;
    [HideInInspector]
    public bool forceEndDodge;

    void FixedUpdate()
    {
        nextState = currState; // do this so all you have to do to change state during the update loop is set nextState to something new

        // ############## pick the proper update loop based on the current state ##############
        switch (currState)
        {
            case State.idle:
                if (camScript.currState == CameraController.State.target)
                    SetTargetYawFromVelocity(camScript.currentTarget.transform.position - transform.position);
                if (camScript.GetWorldMovementVector() != Vector3.zero)
                    nextState = State.walk;
                if (Input.GetMouseButton(0))
                    nextState = State.attack;
                if (Input.GetKey(KeyCode.LeftShift))
                    nextState = State.dodge;
                break;

            case State.walk:

                Vector3 vel = camScript.GetWorldMovementVector() * walkSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + vel);

                if (camScript.GetWorldMovementVector() == Vector3.zero)
                    nextState = State.idle;
                else
                {
                    if (camScript.currState == CameraController.State.target)
                        SetTargetYawFromVelocity(camScript.currentTarget.transform.position - transform.position);
                    else
                        SetTargetYawFromVelocity(vel);
                }
                if (Input.GetMouseButton(0))
                    nextState = State.attack;
                if (Input.GetKey(KeyCode.LeftShift))
                    nextState = State.dodge;

                break;

            case State.attack:
                if (!Input.GetMouseButton(0)) attackButtonReleased = true;
                if (attackButtonReleased && Input.GetMouseButton(0)) bufferAnotherAttack = true;
                if (attackTimer > attackDuration)
                {
                    if (bufferAnotherAttack) { restartState = true; /*print("doing another attack");*/ }
                    else { nextState = State.idle; /*print("going back to idle");*/ }
                }
                attackTimer += Time.deltaTime;
                break;

            case State.dodge:
                break;

            default:
                Debug.LogError("bad");
                break;
        }

        // ############## change state, if necessary ##############
        if (currState != nextState || restartState)
        {
            switch (nextState)
            {
                case State.idle:
                    //animator.CrossFadeInFixedTime("idle", oneFrame * 2f);
                    break;

                case State.walk:
                    animator.CrossFadeInFixedTime("walk", oneFrame * 2f);
                    //impactAnimator.CrossFade("impact", 0);
                    break;

                case State.attack:

                    attackButtonReleased = false;
                    bufferAnotherAttack = false;
                    attackTimer = 0;
                    slashScript.Play();
                    if (attackFlipFlop) animator.CrossFadeInFixedTime("attack right", oneFrame);
                    else animator.CrossFadeInFixedTime("attack left", oneFrame);
                    attackFlipFlop = !attackFlipFlop;

                    if (camScript.currState != CameraController.State.target) 
                    { 
                        Vector3 moveVec = camScript.GetWorldMovementVector();
                        if (moveVec != Vector3.zero) SetTargetYawFromVelocity(moveVec);
                        rb.rotation = Quaternion.Euler(0, targetYaw, 0); // snap
                    }

                    break;

                case State.dodge:
                    break;

                default:
                    Debug.LogError("oof");
                    break;
            }

            currState = nextState;
            restartState = false;
        }

        // rotate to face correct direction (a State-independent behavior)
        float angle = Mathf.MoveTowardsAngle(rb.rotation.eulerAngles.y, targetYaw, turnRate * Time.fixedDeltaTime);
        rb.MoveRotation(Quaternion.Euler(0, angle, 0));
    }

    private void SetTargetYawFromVelocity(Vector3 velocity)
    {
        velocity = new Vector3(velocity.x, 0, velocity.z);
        targetYaw = Vector3.SignedAngle(Vector3.forward, velocity, Vector3.up);
    }
}
