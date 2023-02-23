using UnityEngine;

public class KeepWalking : MonoBehaviour
{
    public CootsController coots;
    public Animator animator;

    void MaybeKeepWalking()
    {
        if (coots.currState == CootsController.State.walk)
        {
            animator.CrossFade("walk", 0);
            coots.PlayRandomWalkSound();
        }
    }
}
