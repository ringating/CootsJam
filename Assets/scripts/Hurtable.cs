using UnityEngine;

public abstract class Hurtable : MonoBehaviour
{
    public abstract void Hurt(Attack attack);

    public abstract float GetHP();

    public abstract void Heal();
}