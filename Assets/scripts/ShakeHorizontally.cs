using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeHorizontally : MonoBehaviour
{
	public ParticleSystem.MinMaxCurve magnitudeScalarOverDuration;
	private float magnitude = 0.5f;
	private float duration;
	private float frequency = 15f;

	float timer = 0;

	private void Update()
	{

		if (timer > 0)
		{
			timer -= Time.unscaledDeltaTime;

			float dx = GetDeltaAtYOffset(5);
			float dz = GetDeltaAtYOffset(10);

			transform.localPosition = new Vector3(dx, 0, dz);
		}
		else 
		{
			transform.localPosition = Vector3.zero;
		}
	}

	private float GetDeltaAtYOffset(float yOffset)
	{
		return magnitudeScalarOverDuration.Evaluate(1 - timer/duration) * magnitude * (Mathf.PerlinNoise(Time.unscaledTime * frequency, yOffset) - 0.5f);
	}

	public void Shake(float duration)
	{
		this.duration = duration;
		timer = duration;
	}
}
