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
	public float hitstopDuration = 0.25f;
	public float hitShakeDuration = 1f;
	[HideInInspector]
	public bool active = false;

	private List<Hurtable> hitList;

	private void Start()
	{
		hitList = new List<Hurtable>();
		active = false; // just for safety
	}

	private void OnTriggerStay(Collider other)
	{
		if (active)
		{
			Hurtable h = other.GetComponent<Hurtable>();
			if (h && !hitList.Contains(h))
			{
				h.Hurt(this);
				hitList.Add(h);
				HitStop.instance.ApplyHitStop(hitstopDuration);
			}
		}
	}

	public void ClearHitList()
	{
		hitList.Clear();
	}
}
