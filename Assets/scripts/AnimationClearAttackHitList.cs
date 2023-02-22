using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClearAttackHitList : MonoBehaviour
{
    [SerializeField]
    private Attack toClear;

    public void ClearHitList()
    {
        toClear.ClearHitList();
    }
}
