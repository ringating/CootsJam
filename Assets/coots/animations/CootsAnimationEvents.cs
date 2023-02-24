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
}
