using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpiderSlash : MonoBehaviour
{
    public Animator slashAnimator; 

    public void PlaySpiderSwordAttackSlash() 
    {
        slashAnimator.CrossFade("spider slash", 0);
    }
}
