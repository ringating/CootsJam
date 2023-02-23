using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEvents : MonoBehaviour
{
    public CootsController coots;

    public void CanCancelHurtWithDodge()
    {
        coots.canDodgeOutOfHurt = true;
    }

    public void StopHurtMovement()
    {
        coots.stopHurtMovement = true;
    }

    public void ForceEndHurt()
    {
        coots.forceEndHurt = true;
    }
}
