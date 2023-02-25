using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFov : MonoBehaviour
{
    public static CameraFov instance;
	
	public Animator fovAnimator;

	CootsController.State prevState;

	private void Start()
	{
		if (instance)
		{
			Debug.LogError("there is more than 1 instance of CameraFov!");
		}
		instance = this;

		prevState = CootsController.instance.currState;
	}

	private void Update()
	{
		if (EnteringKatanaStance()) 
		{
			TightenFov();
		}

		if (LeavingKatanaStance())
		{
			UntightenFov();
		}

		prevState = CootsController.instance.currState;
	}

	private bool EnteringKatanaStance()
	{
		return
			prevState != CootsController.State.katanaStance
			&& CootsController.instance.currState == CootsController.State.katanaStance;
	}

	private bool LeavingKatanaStance()
	{
		return
			prevState == CootsController.State.katanaStance
			&& CootsController.instance.currState != CootsController.State.katanaStance;
	}

	public void TightenFov()
	{
		fovAnimator.CrossFadeInFixedTime("transition to tight fov", CootsController.oneFrame);
	}

	public void TightenFovFast()
	{
		fovAnimator.CrossFadeInFixedTime("fast transition to tight fov", 0);
	}

	public void UntightenFov()
	{
		fovAnimator.CrossFadeInFixedTime("transition to default fov from tight", CootsController.oneFrame);
	}
}
