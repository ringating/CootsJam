using UnityEngine;

public class PlayShockwave : MonoBehaviour
{
    public Animator shockwaveAnimator;
    public Transform spawnPos;

	private void Start()
	{
        if (!shockwaveAnimator || !spawnPos) Debug.LogError("things aren't linked up!");
	}

	public void Play()
    {
        shockwaveAnimator.transform.position = spawnPos.position;
        shockwaveAnimator.transform.rotation = spawnPos.rotation;
        shockwaveAnimator.CrossFade("shockwave", 0);
    }

    public void ForceEnd()
    {
        shockwaveAnimator.CrossFade("shockwave hidden", 0);
    }
}
