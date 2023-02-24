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

    public enum State 
    {
        idle,
        swordAttack
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

    [HideInInspector]
    public State currState;
    private State nextState;
    
    void Start()
    {
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

    float targetYaw;

    void FixedUpdate()
    {
        nextState = currState;

        switch (currState)
        {
            case State.idle:
                if (Input.GetKey(KeyCode.P))
                {
                    nextState = State.swordAttack;
                }
                break;

            case State.swordAttack:
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
                    break;
            }
        }

        currState = nextState;
    }

    public override void Hurt(Attack attack)
    {
        shake.Shake(attack.hitShakeDuration);
        CameraShake.instance.AddShake(attack.camShakeDuration, attack.camShakeMagnitude, attack.camShakeFrequency);
        audioSource.PlayOneShot(attack.toPlayOnHit, attack.toPlayOnHitVolume);
    }
}
