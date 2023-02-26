using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    [HideInInspector]
    public Transform respawnPoint;

    public Animator animator;

    public void Respawn()
    {
        CootsController.instance.transform.position = respawnPoint.position;
        CootsController.instance.targetYaw = CootsController.VectorToYaw(respawnPoint.forward);
        CootsController.instance.transform.rotation = respawnPoint.rotation;
        Death.instance.TriggerRespawnEvents();

        // this might be enough to interrupt fall respawns that might occur later than this respawn
        CootsController.instance.currState = CootsController.State.idle;
        CootsController.instance.animator.CrossFade("idle", 0);
    }
}
