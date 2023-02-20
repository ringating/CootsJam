using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CootsController : MonoBehaviour
{
    const float oneFrame = 1f / 60f;

    public enum State
    {
        idle,
        move,
    }

    public State currState;
    private State nextState;
    
    CharacterController cc;
    public Animator animator;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        nextState = currState; // do this so all you have to do to change state during the update loop is set nextState to something new

        // pick the proper update loop based on the current state
        switch (currState)
        {
            case State.idle:
                if (Input.GetKey(KeyCode.W))
                    nextState = State.move;
                break;

            case State.move:
                if (!Input.GetKey(KeyCode.W))
                    nextState = State.idle;
                break;

            default:
                Debug.LogError("bad");
                break;
        }

        // change state, if necessary
        if (currState != nextState)
        {
            switch (nextState)
            {
                case State.idle:
                    animator.CrossFadeInFixedTime("idle", oneFrame * 2f);
                    break;

                case State.move:
                    animator.CrossFadeInFixedTime("walk", oneFrame * 2f);
                    break;

                default:
                    Debug.LogError("oof");
                    break;
            }

            currState = nextState;
        }
    }
}
