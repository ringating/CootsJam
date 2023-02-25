using UnityEngine;

public class Hand : Hurtable
{
    public Rigidbody rb;
    public ShakeX shake;
    public AudioSource audioSource;

    public enum MovementStyle 
    {
        relative,
        still
    }

    [HideInInspector]
    public MovementStyle currMovementStyle;

    public ParticleSystem.MinMaxCurve lerpLikeCurve;

    Vector3 oldPosition;
    Vector3 targetPosition;
    float moveDurationToTargetPosition;

    float timer;

	private void Awake()
	{
        oldPosition = transform.position;
        targetPosition = transform.position;

        currMovementStyle = MovementStyle.still;

        timer = 0;
        moveDurationToTargetPosition = 1;
    }

	public void MoveToPositionRelativeToCoots(Vector3 relativeToCoots, float moveTime)
    {
        oldPosition = rb.position;
        currMovementStyle = MovementStyle.relative;
        targetPosition = relativeToCoots;
        moveDurationToTargetPosition = Mathf.Max(CootsController.oneFrame, moveTime);
        timer = 0;
    }

    public void MoveToFixedPosition(Vector3 fixedPosition, float moveTime)
    {
        oldPosition = rb.position;
        currMovementStyle = MovementStyle.still;
        targetPosition = fixedPosition;
        moveDurationToTargetPosition = Mathf.Max(CootsController.oneFrame, moveTime);
        timer = 0;
    }

    private void FixedUpdate()
	{
        switch (currMovementStyle) 
        {
            case MovementStyle.relative:

                rb.MovePosition(Vector3.Lerp(
                    oldPosition, 
                    targetPosition + CootsController.instance.rb.position, 
                    lerpLikeCurve.Evaluate(timer / moveDurationToTargetPosition) 
                ));

                break;

            case MovementStyle.still:
                rb.MovePosition(Vector3.Lerp(
                    oldPosition,
                    targetPosition,
                    lerpLikeCurve.Evaluate(timer / moveDurationToTargetPosition)
                ));
                break;
        }

        timer += Time.fixedDeltaTime;
	}

	private void OnDrawGizmos()
	{
        Gizmos.DrawSphere(transform.position, 0.125f);
	}

	public override void Hurt(Attack attack)
	{
        shake.Shake(attack.hitShakeDuration);
        CameraShake.instance.AddShake(attack.camShakeDuration, attack.camShakeMagnitude, attack.camShakeFrequency);
        audioSource.PlayOneShot(attack.toPlayOnHit, attack.toPlayOnHitVolume);
    }
}