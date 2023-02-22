using UnityEngine;

// this isn't perfectly smooth, just rely on the RB version tbh

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
		bool special = false;
		if (timer > timeBetweenLerps)
		{
			//print(timer);
			timer = 0;

			prevPos = nextPos;
			nextPos = Vector3.Lerp(transform.position, target.position, lerpAlpha);

			special = true;
		}
		timer += Time.deltaTime;

		float x = transform.position.x;
		transform.position = Vector3.Lerp(prevPos, nextPos, timer/timeBetweenLerps);
		if (special)
			Debug.LogWarning($"{transform.position.x - x}");
		else
			print($"{transform.position.x - x}");
	}
}
