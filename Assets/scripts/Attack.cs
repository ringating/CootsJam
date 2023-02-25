using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public enum Type
	{
		melee,
		ranged
	}
	
	public Type type;
	public float damage = 45f;
	public Hurtable attackOwner;
	public float hitstopDuration = 0.25f;
	public float hitShakeDuration = 1f;
	public float camShakeDuration = 0.2f;
	public float camShakeFrequency = 10f;
	public float camShakeMagnitude = 5f;
	public AudioClip toPlayOnHit;
	public float toPlayOnHitVolume = 0.8f;
	[HideInInspector]
	public bool active = false;

	private List<Hurtable> hitList;
	private float timer;

	private void Start()
	{
		hitList = new List<Hurtable>();
		active = false; // just for safety
		timer = 0;
	}

	private void FixedUpdate()
	{
		if (timer > 0)
		{
			timer -= Time.fixedDeltaTime;
			if (timer <= 0)
			{
				active = false;
				ClearHitList();
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (active)
		{
			Hurtable h = other.GetComponent<Hurtable>();
			if (h && !hitList.Contains(h))
			{
				HitStop.instance.ApplyHitStop(hitstopDuration);
				h.Hurt(this);
				hitList.Add(h);
			}
		}
	}

	public void ClearHitList()
	{
		hitList.Clear();
	}

	public void ActivateWithLifetime(float lifetime)
	{
		active = true;
		timer = lifetime;
	}
}
