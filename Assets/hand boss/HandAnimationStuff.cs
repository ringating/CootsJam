using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationStuff : MonoBehaviour
{
	public Hand hand;

	public MeshRenderer idle;
	public MeshRenderer chop;
	public MeshRenderer gun;

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
}
