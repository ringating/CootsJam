using UnityEngine;

public class HitPoints : MonoBehaviour
{
    public float maxHP = 100;

    private float _hp;
    public float hp 
    {
        get 
        {
            return _hp;
        }

        set 
        {
            _hp = value;
            if (_hp <= 0)
            {
                Died?.Invoke();
            }
        } 
    }

    public delegate void asjndgfhjkn();
    public event asjndgfhjkn Died;

    public void HealFull()
    {
        hp = maxHP;
    }
}
