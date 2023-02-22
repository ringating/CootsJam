using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public float lerpAlpha = 0.5f;
    public Transform target;

    private const float timeBetweenLerps = 1f/60f;
    private float timer = 0;
	private Vector3 prevPos;
	private Vector3 nextPos;

	private void Start()
	{
		prevPos = transform.position;
		nextPos = Vector3.Lerp(transform.position, target.position, lerpAlpha);
	}

	private void Update()
	{
		if (timer > timeBetweenLerps)
		{
			timer = 0;

			prevPos = transform.position;
			nextPos = Vector3.Lerp(transform.position, target.position, lerpAlpha);
		}
		timer += Time.deltaTime;

		transform.position = Vector3.Lerp(prevPos, nextPos, timer/timeBetweenLerps);
	}
}
