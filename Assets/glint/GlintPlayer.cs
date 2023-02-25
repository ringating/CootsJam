using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlintPlayer : MonoBehaviour
{
    public Animator glintAnimator;

    private void Start()
    {
        if (!glintAnimator) Debug.LogError("things aren't linked up!");
    }

    public void Play()
    {
        glintAnimator.transform.rotation = Quaternion.LookRotation(CootsController.instance.transform.position - glintAnimator.transform.position);
        glintAnimator.CrossFade("glint", 0);
    }
}
