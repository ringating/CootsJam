using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFov : MonoBehaviour
{
    public Animator fovAnimator;

	CootsController.State prevState;

	private void Start()
	{
		prevState = CootsController.instance.currState;
	}

	private void Update()
	{
		if (prevState != CootsController.State.katanaStance &&
			CootsController.instance.currState == CootsController.State.katanaStance) 
		{
			fovAnimator.CrossFadeInFixedTime("transition to tight fov", CootsController.oneFrame);
		}

		if (prevState == CootsController.State.katanaStance &&
			CootsController.instance.currState != CootsController.State.katanaStance)
		{
			fovAnimator.CrossFadeInFixedTime("transition to default fov from tight", CootsController.oneFrame);
		}

		prevState = CootsController.instance.currState;
	}
}
