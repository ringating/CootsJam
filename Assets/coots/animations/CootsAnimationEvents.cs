using System.Collections.Generic;
using UnityEngine;

public class CootsAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private Animator gunShockwaveAnimator;

    public AudioClip dryFire;
    public const float dryFireVol = 1f;

    public void SuggestEnemiesUseRangedAttacks() 
    {
        CootsController.instance.heldKatanaStanceForAWhile = true;
    }

    public void RespawnFromFall()
    {
        CootsController.instance.RespawnFromFall();
    }

    private bool shotSuccessfully;
    public void ResetSuccessfulShotBool()
    {
        shotSuccessfully = false;
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

            shotSuccessfully = true;

            CootsController.instance.gunAttack.transform.position = onScreenVulnerableTargets[0].transform.position;
            CootsController.instance.gunAttack.ActivateWithLifetime(3 * CootsController.oneFrame);
            float yaw = CootsController.VectorToYaw(onScreenVulnerableTargets[0].transform.position - CootsController.instance.transform.position);
            CootsController.instance.targetYaw = yaw;
            CootsController.instance.transform.rotation = Quaternion.Euler(0, yaw, 0);
            CootsController.instance.audioSource.PlayOneShot(CootsController.instance.gunshot, CootsController.gunshotVol);

            onScreenVulnerableTargets[0].InterruptRangedAttack();

            CameraFov.instance.TightenFovFast();

            gunShockwaveAnimator.CrossFade("shockwave", 0);

            //print("bang");
        }
        else
        {
            CootsController.instance.audioSource.PlayOneShot(dryFire, dryFireVol);
        }
    }

    public void EarlyExitShotIfSuccessful()
    {
        if (shotSuccessfully)
        {
            CootsController.instance.animator.CrossFade("idle", 0);
            CootsController.instance.currState = CootsController.State.idle;
            CameraFov.instance.UntightenFov();
        }
    }

    public void ReturnToIdleState()
    {
        CootsController.instance.currState = CootsController.State.idle;
    }
}
