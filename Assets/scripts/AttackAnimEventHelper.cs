using UnityEngine;

public class AttackAnimEventHelper : MonoBehaviour
{
    public Attack attack;

    /*public void AttackActivate()
    {
        attack.active = true;
    }

    public void AttackDeactivate()
    {
        attack.active = false;
    }*/

    public void AttackClearHitList() 
    {
        attack.ClearHitList();
    }
}
