using UnityEngine;

public class DodgeEvents : MonoBehaviour
{
    public CootsController coots;

    public void EndInvincibility()
    {
        coots.invincible = false;
    }

    public void MakeCancellable()
    {
        coots.canCancelDodge = true;
    }

    public void EndDodgeState()
    {
        coots.forceEndDodge = true;
    }
}
