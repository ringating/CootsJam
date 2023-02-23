using UnityEngine;

public class SmoothFollowKinematicRB : MonoBehaviour
{
    public float lerpAlpha = 0.1f;
    public Transform target;

	public bool followPos = true;
	public bool followRot = false;

	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if(followPos)
			rb.MovePosition(Vector3.Lerp(rb.position, target.position, lerpAlpha));

		if (followRot)
			rb.MoveRotation(Quaternion.Slerp(rb.rotation, target.rotation, lerpAlpha));
	}

	public void SnapToTarget()
	{
		if (followPos)
			rb.position = target.position;

		if (followRot)
			rb.rotation = target.rotation;
	}
}
