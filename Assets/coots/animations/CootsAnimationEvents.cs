using System.Collections.Generic;
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
        Targetable[] targets = FindObjectsOfType<Targetable>();
        List<Targetable> onScreenVulnerableTargets = new List<Targetable>();
        foreach (Targetable t in targets)
        {
            if (t.vulnerableToGun && CameraController.PositionIsOnScreen(t.transform.position))
                onScreenVulnerableTargets.Add(t);
        }

        if (onScreenVulnerableTargets.Count > 0)
        {
            
        }
    }

    public void ReturnToIdleState()
    {
        CootsController.instance.currState = CootsController.State.idle;
    }
}
