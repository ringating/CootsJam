using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CootsController : MonoBehaviour
{
    public float attackDuration = 0.27f;

    const float oneFrame = 1f / 60f;

    public enum State
    {
        idle,
        walk,
        attack,
    }

    [HideInInspector]
    public State currState;
    private State nextState;
    private bool restartState = false;
    
    CharacterController cc;
    public Animator animator;
    public Animator impactAnimator;
    public Slash slashScript;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // ############## per-state vars ##############

    // attack
    bool attackButtonReleased = false;
    bool bufferAnotherAttack = false;
    float attackTimer = 0;
    bool attackFlipFlop;

    void FixedUpdate()
    {
        nextState = currState; // do this so all you have to do to change state during the update loop is set nextState to something new

        // ############## pick the proper update loop based on the current state ##############
        switch (currState)
        {
            case State.idle:
                if (Input.GetKey(KeyCode.W)) nextState = State.walk;
                if (Input.GetMouseButton(0)) nextState = State.attack;
                break;

            case State.walk:
                if (!Input.GetKey(KeyCode.W)) nextState = State.idle;
                if (Input.GetMouseButton(0)) nextState = State.attack;
                break;

            case State.attack:
                if (!Input.GetMouseButton(0)) attackButtonReleased = true;
                if (attackButtonReleased && Input.GetMouseButton(0)) bufferAnotherAttack = true;
                if (attackTimer > attackDuration)
                {
                    if (bufferAnotherAttack) { restartState = true; print("doing another attack"); }
                    else { nextState = State.idle; print("going back to idle"); }
                }
                attackTimer += Time.deltaTime;
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
                    impactAnimator.CrossFade("impact", 0);
                    break;

                case State.attack:
                    attackButtonReleased = false;
                    bufferAnotherAttack = false;
                    attackTimer = 0;
                    slashScript.Play();
                    if (attackFlipFlop) animator.CrossFadeInFixedTime("attack right", oneFrame);
                    else animator.CrossFadeInFixedTime("attack left", oneFrame);
                    attackFlipFlop = !attackFlipFlop;
                    break;

                default:
                    Debug.LogError("oof");
                    break;
            }

            currState = nextState;
            restartState = false;
        }
    }
}
