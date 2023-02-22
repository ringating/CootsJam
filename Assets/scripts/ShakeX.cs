using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeX : MonoBehaviour
{
	public ParticleSystem.MinMaxCurve magnitudeScalarOverDuration;
	private float magnitude = 0.025f;
	private float frequency = 15f;
	private float duration;

	float timer = 999999;

	private void Update()
	{

		if (timer < duration)
		{
			timer += Time.unscaledDeltaTime;

			float dx = GetDeltaX();

			transform.localPosition = new Vector3(dx, 0, 0);
		}
		else 
		{
			transform.localPosition = Vector3.zero;
		}
	}

	private float GetDeltaX()
	{
		return Mathf.Sqrt(magnitudeScalarOverDuration.Evaluate(timer/duration) * magnitude) * Mathf.Cos(Time.unscaledTime * 2 * Mathf.PI * frequency);
	}

	public void Shake(float duration)
	{
		this.duration = duration;
		timer = 0;
	}
}
