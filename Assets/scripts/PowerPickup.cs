using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPickup : MonoBehaviour
{
	public GameObject uIToEnable;

	public enum Type 
	{
		katana, gun
	}
	public Type type;

	private void Start()
	{
		uIToEnable.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("coots"))
		{
			HitStop.instance.paused = true;
			uIToEnable.SetActive(true);
			Cursor.lockState = CursorLockMode.None;

			if (type == Type.gun)
			{
				CootsController.instance.hasGun = true;
			}
			else 
			{
				CootsController.instance.hasKatana = true;
			}

			gameObject.SetActive(false);
		}
	}
}
