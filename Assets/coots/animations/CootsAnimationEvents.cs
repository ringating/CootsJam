using UnityEngine;

public class CootsAnimationEvents : MonoBehaviour
{
    public void SuggestEnemiesUseRangedAttacks() 
    {
        CootsController.instance.heldKatanaStanceForAWhile = true;
    }

    public void RespawnFromFall()
    {
        CootsController.instance.RespawnFromFall();
    }

    public void TryShot()
    {

    }

    public void ReturnToIdleState()
    {
        CootsController.instance.currState = CootsController.State.idle;
    }
}
