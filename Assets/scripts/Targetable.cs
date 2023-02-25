using UnityEngine;

public class Targetable : MonoBehaviour
{
    public bool vulnerableToGun { get; set; }

    public delegate void Asdf();
    public event Asdf OnInterrupted;

    public void InterruptRangedAttack()
    {
        OnInterrupted?.Invoke();
    }
}
