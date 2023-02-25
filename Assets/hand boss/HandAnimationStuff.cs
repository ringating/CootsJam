using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationStuff : MonoBehaviour
{
	public Hand hand;

	public MeshRenderer idle;
	public MeshRenderer chop;
	public MeshRenderer gun;

	public AudioSource audioSource;
	public AudioClip fingerGunSound;
	public AudioClip alertSound;

	const float fingerGunSoundVol = 0.8f;
	const float alertSoundVol = 0.8f;

	public void StopMoving()
	{
		hand.MoveToFixedPosition(hand.rb.position, 0);
	}

	public void MoveToCootsOverTime(float time)
	{
		hand.MoveToPositionRelativeToCoots(Vector3.zero, time);
	}

	public enum HandSprite
	{
		idle, chop, gun
	}

	public void ChangeSprite(HandSprite to)
	{
		idle.enabled = false;
		chop.enabled = false;
		gun.enabled = false;

		switch (to)
		{
			case HandSprite.idle:
				idle.enabled = true;
				break;

			case HandSprite.chop:
				chop.enabled = true;
				break;

			case HandSprite.gun:
				gun.enabled = true;
				break;
		}
	}

	public PlayShockwave shockwaveScript;
	public Attack fingerGunAttack;
	public void PlayGunShockwave() // aka Shoot()
	{
		shockwaveScript.Play();
		audioSource.PlayOneShot(fingerGunSound, fingerGunSoundVol);

		fingerGunAttack.transform.position = CootsController.instance.transform.position;
		fingerGunAttack.transform.rotation = Quaternion.Euler(0, CootsController.VectorToYaw(CootsController.instance.transform.position - shockwaveScript.transform.position), 0);
		fingerGunAttack.ActivateWithLifetime(CootsController.oneFrame * 2);
	}

	public void EndGunShockwave() 
	{
		shockwaveScript.ForceEnd();
	}

	public GlintPlayer glintScript;
	public void PlayGunGlint()
	{
		glintScript.Play();
		audioSource.PlayOneShot(alertSound, alertSoundVol);
	}
}
