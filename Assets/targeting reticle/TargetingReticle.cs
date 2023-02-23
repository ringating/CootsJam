using UnityEngine;
using UnityEngine.UI;

public class TargetingReticle : MonoBehaviour
{
    public CameraController cameraScript;
    public Animator reticleAnimator;

    CameraController.State prevState;

	private void Start()
	{
        prevState = cameraScript.currState;
    }

	void Update()
    {
        switch (cameraScript.currState)
        {
            case CameraController.State.free:
                if (prevState == CameraController.State.target)
                {
                    reticleAnimator.CrossFade("fade out", 0);
                }
                break;

            case CameraController.State.target:

                if (prevState == CameraController.State.free)
                {
                    reticleAnimator.CrossFade("fade in", 0);
                }

                transform.position = Camera.main.WorldToScreenPoint(cameraScript.currentTarget.transform.position);

                break;
        }

        prevState = cameraScript.currState;
    }
}
