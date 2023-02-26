using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : Hurtable
{
	public MeshRenderer mr;

	public Texture2D crackedOnce;
	public Texture2D crackedTwice;

	public Collider col;

	public AudioSource audioSource;

	public ShakeX shake;

	public Transform firstTutorialThing;

	public HandAnimationStuff handAnimStuff;

	int hp;

	private void Start()
	{
		hp = 3;

		mr.material = Instantiate(mr.material);
	}

	public override void Hurt(Attack attack)
	{
		hp -= 1;

		switch (hp)
		{
			case 2:
				mr.material.mainTexture = crackedOnce;
				break;

			case 1:
				mr.material.mainTexture = crackedTwice;
				break;

			case 0:
				transform.rotation = Quaternion.Euler(90,0,0);
				col.enabled = false;
				firstTutorialThing.transform.position = Vector3.down * 1000f;
				handAnimStuff.FirstSpawn();
				Flags.encounteredFirstBoss = true;
				break;
		}

		shake.Shake(attack.hitShakeDuration);
		audioSource.PlayOneShot(attack.toPlayOnHit, attack.toPlayOnHitVolume);
	}

	public override float GetHP()
	{
		print("why");
		return hp;
	}

	public override void Heal()
	{
		
	}
}
