using UnityEngine;

public class SmoothFollowKinematicRB : MonoBehaviour
{
    public float lerpAlpha = 0.1f;
    public Transform target;

	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		rb.MovePosition(Vector3.Lerp(rb.position, target.position, lerpAlpha));
	}

	public void SnapToTarget()
	{
		rb.position = target.position;
	}
}
