using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimationEvents : MonoBehaviour
{
    public Spider spider;

    public void ForceReturnToIdleState() 
    {
        spider.currState = Spider.State.idle;
        spider.animator.CrossFade("idle bob", 0);
    }
}
