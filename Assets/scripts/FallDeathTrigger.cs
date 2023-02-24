using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeathTrigger : MonoBehaviour
{
	public Transform respawnPoint;

	private void Start()
	{
		if (!respawnPoint)
			Debug.LogError("this FallDeathTrigger lacks a respawn point!");
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("coots")) 
		{
			if (CootsController.instance.currState != CootsController.State.falling)
			{
				CootsController.instance.fallRespawnPoint = respawnPoint.position;
				CootsController.instance.Fall();
			}
		}
	}
}
