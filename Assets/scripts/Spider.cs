using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Hurtable
{
    public GameObject topHat;
    public GameObject monocle;
    public GameObject eyepatch;
    public GameObject sword;
    public GameObject musket;
    public Rigidbody rb;
    public Transform shotAnchor;
    public Targetable targetable;

    public float aggroRange;
    public float attackRange = 4f;
    public float attackAngleRange = 90f;
    public float moveSpeed = 6f;
    public float turnRate = 360f;

    public enum State 
    {
        idle,
        swordAttack,
        swordAggro,
        musketAggro,
        musketAttack,
    }

    public enum Type 
    {
        sword,
        musket
    }

    public Type type;

    public Animator animator;
    public ShakeX shake;
    public AudioSource audioSource;
    public AudioClip musketAlert;
    public AudioClip musketShot;
    public AudioClip swordAlert;
    public AudioClip swordSwing;

    public const float musketAlertVol = 0.8f;
    public const float musketShotVol = 0.5f;
    public const float swordAlertVol = 0.8f;
    public const float swordSwingVol = 0.8f;

    [HideInInspector]
    public State currState;
    private State nextState;
    private Vector3 spawnPos;
    [HideInInspector]
    public bool move;
    
    void Start()
    {
        spawnPos = transform.position;

        switch (type)
        {
            case Type.sword:

                eyepatch.SetActive(true);
                sword.SetActive(true);

                topHat.SetActive(false);
                monocle.SetActive(false);
                musket.SetActive(false);

                break;


            case Type.musket:

                eyepatch.SetActive(false);
                sword.SetActive(false);

                topHat.SetActive(true);
                monocle.SetActive(true);
                musket.SetActive(true);

                break;
        }
    }

    //float targetYaw;

    void FixedUpdate()
    {
        nextState = currState;

        switch (currState)
        {
            case State.idle:
                break;

            case State.swordAggro:
                if (move)
                {
                    HopMovement();
                }
                else
                {
                    if (CootsInRange())
                    {
                        nextState = State.swordAttack;
                    }
                }
                break;

            case State.musketAggro:
                if (move)
                {
                    HopMovement();
                }
                else
                {
                    if (CootsInRange())
                    {
                        nextState = State.musketAttack;
                    }
                }
                break;
        }

        if (nextState != currState)
        {
            switch (nextState) 
            {
                case State.idle:
                    break;

                case State.swordAttack:
                    animator.CrossFade("spider sword swing", 0);
                    audioSource.PlayOneShot(swordAlert, swordAlertVol);
                    break;

                case State.musketAttack:
                    animator.CrossFade("musket attack", 0);
                    audioSource.PlayOneShot(musketAlert, musketAlertVol);
                    break;
            }
        }

        currState = nextState;
    }

    private void HopMovement()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, CootsController.instance.transform.position, moveSpeed * Time.fixedDeltaTime));

        float yaw = rb.rotation.eulerAngles.y;
        float targetYaw = yaw + GetSignedAngleToCoots();
        float nextYaw = Mathf.MoveTowardsAngle(yaw, targetYaw, turnRate * Time.fixedDeltaTime);
        rb.MoveRotation(Quaternion.Euler(0, nextYaw, 0));
    }

    private bool CootsInRange()
    {
        return 
            Vector3.Distance(rb.position, CootsController.instance.rb.position) < attackRange &&
            GetAngleToCoots() < attackAngleRange / 2f;
    }

    public override void Hurt(Attack attack)
    {
        shake.Shake(attack.hitShakeDuration);
        CameraShake.instance.AddShake(attack.camShakeDuration, attack.camShakeMagnitude, attack.camShakeFrequency);
        audioSource.PlayOneShot(attack.toPlayOnHit, attack.toPlayOnHitVolume);
    }

    public void IdleDecision()
    {
        //print("idle decision");

        // maybe aggro
        if (Vector3.Distance(CootsController.instance.transform.position, transform.position) < aggroRange)
        {
            switch (type)
            {
                case Type.sword:
                    currState = State.swordAggro;
                    animator.CrossFadeInFixedTime("hopMovement", 2 * CootsController.oneFrame);
                    break;

                case Type.musket:
                    currState = State.musketAggro;
                    animator.CrossFadeInFixedTime("hopMovement", 2 * CootsController.oneFrame);
                    break;
            }
        }
    }

    public void PlaySwordSwingSound()
    {
        audioSource.PlayOneShot(swordSwing, swordSwingVol);
    }

    public float GetAngleToCoots()
    {
        return Vector3.Angle(-transform.forward, CootsController.instance.rb.position - rb.position); // -forward because i accidentally made spider face -z
    }

    public float GetSignedAngleToCoots()
    {
        return Vector3.SignedAngle(-transform.forward, CootsController.instance.rb.position - rb.position, Vector3.up); // -forward because i accidentally made spider face -z
    }

    public void FireMusket()
    {
        FaceCoots();

        shotAnchor.localScale = new Vector3(shotAnchor.localScale.x, shotAnchor.localScale.y, 
            Vector3.Distance(transform.position, CootsController.instance.transform.position) + 3
        );

        audioSource.PlayOneShot(musketShot, musketShotVol);

        targetable.vulnerableToGun = false;
    }

    public void FaceCoots()
    {
        transform.rotation = Quaternion.Euler(0, CootsController.VectorToYaw(rb.position - CootsController.instance.rb.position), 0);
    }
}
