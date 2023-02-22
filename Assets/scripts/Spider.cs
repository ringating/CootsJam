using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Hurtable
{
    public ShakeX shake;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Hurt(Attack attack)
    {
        shake.Shake(attack.hitShakeDuration);
        CameraShake.instance.AddShake(attack.camShakeDuration, attack.camShakeMagnitude, attack.camShakeFrequency);
    }
}
