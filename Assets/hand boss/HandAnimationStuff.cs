using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationStuff : MonoBehaviour
{
	[HideInInspector]
	public float turnRate = 0;
	
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
		targetable.vulnerableToGun = false;

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
		targetable.vulnerableToGun = true;
	}

	public Targetable targetable;
	private void Start()
	{
		targetable.OnInterrupted += InterruptedByCoots;
	}

	public Animator animator;
	public void InterruptedByCoots()
	{
		animator.CrossFade("interrupted", 0);
		targetable.vulnerableToGun = false;
	}

	public Attack chopAttack;
	public void ActivateChopAttack()
	{
		chopAttack.ActivateWithLifetime(CootsController.oneFrame * 2);
	}

	public void MaybePickAnAttack() // 50/50 to do an attack, use this to make the hand sometimes idle for less time
	{
		if (Random.Range(0, 2) > 0)
			PickAnAttack();
	}

	public void PickAnAttack()
	{
		switch (hand.bossVersion) 
		{
			case Hand.BossVersion.melee:
				animator.CrossFade("chop flurry", 0);
				break;

			case Hand.BossVersion.ranged:
				animator.CrossFade("gun flurry", 0);
				break;

			case Hand.BossVersion.final:
				if(Random.Range(0,2) > 0)
					animator.CrossFade("chop flurry", 0);
				else
					animator.CrossFade("gun flurry", 0);
				break;
		}
	}
}
