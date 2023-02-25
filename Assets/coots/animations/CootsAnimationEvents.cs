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
            // idk just shoot an arbitrary one, that's easiest for now
            CootsController.instance.gunAttack.transform.position = onScreenVulnerableTargets[0].transform.position;
            CootsController.instance.gunAttack.ActivateWithLifetime(3 * CootsController.oneFrame);
            float yaw = CootsController.VectorToYaw(onScreenVulnerableTargets[0].transform.position - CootsController.instance.transform.position);
            CootsController.instance.targetYaw = yaw;
            CootsController.instance.transform.rotation = Quaternion.Euler(0, yaw, 0);
            CootsController.instance.audioSource.PlayOneShot(CootsController.instance.gunshot, CootsController.gunshotVol);

            onScreenVulnerableTargets[0].InterruptRangedAttack();

            //print("bang");
        }
    }

    public void ReturnToIdleState()
    {
        CootsController.instance.currState = CootsController.State.idle;
    }
}
