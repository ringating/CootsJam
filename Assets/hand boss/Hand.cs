using UnityEngine;

public class Hand : Hurtable
{
    public Rigidbody rb;
    public ShakeX shake;
    public AudioSource audioSource;
    public HandAnimationStuff animEventsScript;

    public float maxHP = 1000f;
    float hp;

    public Collider col;

    public enum BossVersion 
    {
        melee,
        ranged,
        final
    }

    public BossVersion bossVersion;

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

        col.enabled = false;
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
        float targetYaw = CootsController.VectorToYaw(CootsController.instance.transform.position - transform.position);
        rb.MoveRotation(Quaternion.Euler(0, Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, targetYaw, animEventsScript.turnRate * Time.fixedDeltaTime), 0));

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
        hp -= attack.damage;
        if (hp <= 0)
        {
            switch (bossVersion)
            {
                case BossVersion.melee:
                    Flags.defeatedKatanaBoss = true;
                    break;

                case BossVersion.ranged:
                    Flags.defeatedGunBoss = true;
                    break;

                case BossVersion.final:
                    WinScreen.winScreen.SetActive(true);
                    break;
            }
        }
    }

	public override float GetHP()
	{
        return hp;
	}

	public override void Heal()
	{
        hp = maxHP;
	}
}
