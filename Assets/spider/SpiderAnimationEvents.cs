using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimationEvents : MonoBehaviour
{
    public Spider spider;
    public Attack musketAttack;

    public void ForceReturnToIdleState() 
    {
        spider.currState = Spider.State.idle;
        spider.animator.CrossFade("idle bob", 0);
    }

    public void PlaySwordSwingSound()
    {
        spider.PlaySwordSwingSound();
    }

    public void IdleDecision()
    {
        spider.IdleDecision();
    }

    public void MoveEnable()
    {
        spider.move = true;
    }

    public void MoveDisable()
    {
        spider.move = false;
    }

    public void FireMusket()
    {
        musketAttack.active = true;
        musketAttack.transform.rotation = Quaternion.Euler(0, CootsController.VectorToYaw(CootsController.instance.transform.position - spider.transform.position), 0);
        musketAttack.transform.position = CootsController.instance.transform.position;
        spider.FireMusket();
    }

    public void DeactivateShotAttack()
    {
        musketAttack.active = false;
        musketAttack.ClearHitList();
    }
}
