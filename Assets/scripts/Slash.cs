using UnityEngine;

public class Slash : MonoBehaviour
{
    public Animator animator;
    public Transform slashParent;
    
    bool tiltState;

    public void Play()
    {
        slashParent.localRotation = Quaternion.Euler(-5, 0, tiltState ? -165 : -15);
        tiltState = !tiltState;
        animator.CrossFade("slash", 0);
    }
}
