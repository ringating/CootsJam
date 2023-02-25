using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CootsController : Hurtable
{
    public static CootsController instance;

    public float walkSpeed = 5f;
    public float hurtSpeed = 5f;
    public float iaiStanceSpeed = 1f;
    public float iaiStanceTurnRate = 0f;
    public float iaiTeleportDistance = 4f;
    public float dodgeSpeed = 10f;
    public float dodgeTurnRate = 360f;
    public float turnRate = 720f;
    public float attackDuration = 0.27f;

    public const float oneFrame = 1f / 60f;

    public enum State
    {
        idle,
        walk,
        attack,
        dodge,
        hurt,
        katanaStance,
        //katanaTeleport,
        falling,
        gun,
    }

    [HideInInspector]
    public State currState;
    private State nextState;
    private bool restartState = false;
    [HideInInspector]
    public float targetYaw;
    [HideInInspector]
    public bool heldKatanaStanceForAWhile = false;
    [HideInInspector]
    public bool hasKatana;
    [HideInInspector]
    public bool hasGun;

    public int powerLevel {get{ return (hasGun ? 1 : 0) + (hasKatana ? 1 : 0); }}

    //public CharacterController cc;
    public Rigidbody rb;
    public Animator animator;
    public Animator impactAnimator;
    public Animator fovAnimator;
    public Animator waterfowlAnimator;
    public Slash slashScript;
    public CameraController camScript;
    public AudioSource audioSource;
    public KatanaEffects katanaEffects;
    public Attack gunAttack;

    public AudioClip attackA;
    public AudioClip attackB;
    public AudioClip dodge;
    public AudioClip[] walkSounds;
    public AudioClip stanceEnter;
    public AudioClip stanceExit;
    public AudioClip parry;
    public AudioClip fall;
    public AudioClip gunshot;

    const float attackAVol = 0.8f;
    const float attackBVol = 0.8f;
    const float dodgeVol = 0.6f;
    const float walkVol = 0.4f;
    const float stanceEnterVol = 0.8f;
    const float stanceExitVol = 0.8f;
    const float parryVol = 0.8f;
    const float fallVol = 0.8f;
    public const float gunshotVol = 0.6f;

    void Awake()
    {
        if (instance) 
        {
            Debug.LogError("there is more than 1 instance of CootsController!");
        }
        instance = this;

        parriedAttacks = new List<Attack>();
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
    private bool dodgeFlipFlop;

    // hurt
    [HideInInspector]
    public bool stopHurtMovement;
    [HideInInspector]
    public bool canDodgeOutOfHurt;
    [HideInInspector]
    public bool forceEndHurt;

    // katana stance
    List<Attack> parriedAttacks;

    void FixedUpdate()
    {
        nextState = currState; // do this so all you have to do to change state during the update loop is set nextState to something new

        Vector3 inputVector = camScript.GetWorldMovementVector(); // this gets used a lot, so coming up with a new thing to name it in every case is kinda silly

        if (currState != State.katanaStance) heldKatanaStanceForAWhile = false; // there's no switch for doing things upon leaving state, so i'll just put this here...

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
                if (Input.GetMouseButton(1))
                    nextState = State.katanaStance;
                if (Input.GetKey(KeyCode.E))
                    nextState = State.gun;
                break;

            case State.walk:

                Vector3 vel = inputVector * walkSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + vel);

                if (inputVector == Vector3.zero)
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
                if (Input.GetMouseButton(1))
                    nextState = State.katanaStance;
                if (Input.GetKey(KeyCode.E))
                    nextState = State.gun;

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

                if (forceEndDodge)
                {
                    nextState = State.idle;
                }

                if (canCancelDodge)
                {
                    if (inputVector != Vector3.zero)
                        nextState = State.walk;
                    if (Input.GetMouseButton(0))
                        nextState = State.attack;
                    if (Input.GetKey(KeyCode.LeftShift))
                        restartState = true;
                }
                else
                {
                    // this is the period of time before the dodge lands on the ground
                    if (inputVector.magnitude > 0)
                    {
                        targetYaw = Mathf.MoveTowardsAngle(targetYaw, VectorToYaw(inputVector), dodgeTurnRate * Time.fixedDeltaTime);
                    }
                    Vector3 v = YawToVector(targetYaw) * dodgeSpeed * Time.fixedDeltaTime;
                    rb.MovePosition(rb.position + v);
                }

                break;

            case State.hurt:

                if (!stopHurtMovement)
                {
                    rb.MovePosition(rb.position - (YawToVector(targetYaw) * hurtSpeed * Time.fixedDeltaTime));
                }

                if (forceEndHurt) 
                {
                    nextState = State.idle;
                }

                if (canDodgeOutOfHurt)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        nextState = State.dodge;
                    }
                }

                break;

            case State.katanaStance:

                rb.MovePosition(rb.position + inputVector * iaiStanceSpeed * Time.fixedDeltaTime);

                if (camScript.currState == CameraController.State.target)
                    SetTargetYawFromVelocity(camScript.currentTarget.transform.position - transform.position);
                else if (inputVector != Vector3.zero)
                {
                    //SetTargetYawFromVelocity(inputVector);
                    targetYaw = Mathf.MoveTowardsAngle(targetYaw, VectorToYaw(inputVector), iaiStanceTurnRate * Time.fixedDeltaTime);
                }

                if (!Input.GetMouseButton(1))
                {
                    if (parriedAttacks.Count > 0)
                    {
                        // TODO: counterattack?

                        parriedAttacks.Clear();
                        katanaEffects.Sheathe();

                        nextState = State.idle;
                        animator.CrossFadeInFixedTime("idle", 2 * oneFrame); // same reasoning as below (but this might be temporary, if there's a new behavior for counterattack at some point)
                        audioSource.PlayOneShot(stanceExit, stanceExitVol);
                    }
                    else
                    {
                        nextState = State.idle;
                        animator.CrossFadeInFixedTime("idle", 2 * oneFrame); // since stance is a looping animation w/ no exit time transition back to the idle animation, need to manually transition here
                        audioSource.PlayOneShot(stanceExit, stanceExitVol);
                    }
                }
                break;


            case State.falling:
                // tbh i think this'll be handled entirely by an animation
                break;

            case State.gun:
                break;


            default:
                Debug.LogError("bad");
                break;
        }

        // ############## change state, if necessary ##############
        if (currState != nextState || restartState)
        {
            // state exit cleanup
            switch (currState)
            {
                default:
                    break;
            }

            // state entry steup
            switch (nextState)
            {
                case State.idle:
                    //animator.CrossFadeInFixedTime("idle", oneFrame * 2f);
                    break;

                case State.walk:
                    animator.CrossFadeInFixedTime("walk", oneFrame * 2f);
                    PlayRandomWalkSound();
                    //impactAnimator.CrossFade("impact", 0);
                    break;

                case State.attack:

                    attackButtonReleased = false;
                    bufferAnotherAttack = false;
                    attackTimer = 0;
                    slashScript.Play();
                    if (attackFlipFlop)
                    {
                        animator.CrossFadeInFixedTime("attack right", oneFrame);
                        audioSource.PlayOneShot(attackA, attackAVol);
                    }
                    else 
                    { 
                        animator.CrossFadeInFixedTime("attack left", oneFrame);
                        audioSource.PlayOneShot(attackB, attackBVol);
                    }
                    attackFlipFlop = !attackFlipFlop;

                    // when not targeting, snap to input dir (if there is one). when targeting, snap to target.
                    if (camScript.currState != CameraController.State.target)
                    {
                        if (inputVector != Vector3.zero)
                            SetTargetYawFromVelocity(inputVector);
                    }
                    else
                    {
                        SetTargetYawFromVelocity(camScript.currentTarget.transform.position - transform.position);
                    }
                    rb.rotation = Quaternion.Euler(0, targetYaw, 0); // snap to targetYaw

                    break;

                case State.dodge:

                    animator.CrossFade(dodgeFlipFlop ? "dodge A" : "dodge B", 0);
                    dodgeFlipFlop = !dodgeFlipFlop;
                    forceEndDodge = false;
                    canCancelDodge = false;
                    invincible = true;

                    // if there's input, snap to that direction
                    if (inputVector != Vector3.zero)
                    {
                        SetTargetYawFromVelocity(inputVector);
                        rb.rotation = Quaternion.Euler(0, targetYaw, 0);
                    }

                    audioSource.PlayOneShot(dodge, dodgeVol);

                    break;

                case State.hurt:
                    // all of this takes place in Hurt() instead, since that's the only place that this state is ever entered from
                    break;

                case State.katanaStance:
                    animator.CrossFadeInFixedTime("katana stance", 2 * oneFrame);
                    audioSource.PlayOneShot(stanceEnter, stanceEnterVol);
                    break;

                case State.falling:
                    // all handled by the animation
                    break;

                case State.gun:
                    animator.CrossFadeInFixedTime("gunshot", oneFrame);
                    //audioSource.PlayOneShot(gunshot, gunshotVol);
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

    public static float VectorToYaw(Vector3 v)
    {
        v = new Vector3(v.x, 0, v.z);
        return Vector3.SignedAngle(Vector3.forward, v, Vector3.up);
    }

    public static Vector3 YawToVector(float y)
    {
        return Quaternion.Euler(0, y, 0) * Vector3.forward;
    }

    bool katanaStanceFlipFlop;
	public override void Hurt(Attack attack)
	{
        if (currState == State.katanaStance && attack.type == Attack.Type.melee)
        {
            audioSource.PlayOneShot(parry, parryVol);
            katanaEffects.Unsheathe();
            HitStop.instance.CancelHitStop();

            parriedAttacks.Add(attack);

            waterfowlAnimator.transform.parent.position = transform.position;
            waterfowlAnimator.transform.parent.rotation = transform.rotation;
            waterfowlAnimator.CrossFade(katanaStanceFlipFlop ? "waterfowl A" : "waterfowl B", 0);
            katanaStanceFlipFlop = !katanaStanceFlipFlop;

            Vector3 input = camScript.GetWorldMovementVector();
            if (input != Vector3.zero)
            {
                rb.position += input * iaiTeleportDistance;
            }
            else
            {
                rb.position -= transform.forward * iaiTeleportDistance;
            }
        }
        else if ( (!(currState == State.dodge && invincible)) && (!(currState == State.falling)) )
        {
            animator.CrossFadeInFixedTime("hurt", oneFrame);

            audioSource.PlayOneShot(attack.toPlayOnHit, attack.toPlayOnHitVolume);

            currState = State.hurt; // setting nextState here wouldn't do anything

            SetTargetYawFromVelocity(-attack.transform.forward); // face the reverse direction that the attack is facing 
            rb.rotation = Quaternion.Euler(0, targetYaw, 0); // snap

            stopHurtMovement = false;
            canDodgeOutOfHurt = false;
            forceEndHurt = false;

            CootsHP.instance.DealDamage(attack.damage);
        }
    }

    public void PlayRandomWalkSound()
    {
        int i = Random.Range(0, walkSounds.Length);
        audioSource.PlayOneShot(walkSounds[i], walkVol);
    }

    public void Fall()
    {
        audioSource.PlayOneShot(fall, fallVol);
        animator.CrossFade("fall", 0);
        currState = State.falling;
        CootsHP.instance.DealDamage(10f);
    }

    public Vector3 fallRespawnPoint;

    public void RespawnFromFall()
    {
        currState = State.idle;

        rb.position = new Vector3(fallRespawnPoint.x, 0, fallRespawnPoint.z); // just assumes the player is always at y=0, but change this if that ever becomes not always true
    }
}
