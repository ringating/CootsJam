using UnityEngine;

public class KatanaEffects : MonoBehaviour
{
    public MeshRenderer mr;
    public MeshRenderer cootsFront;
    public MeshRenderer cootsBack;
    
    public Texture2D sheathed;
    public Texture2D unsheathed;
    public float blinkDuration;
    public int blinkCount;

    float timer;

	private void Start()
	{
        mr.material = Instantiate(mr.material);
	}

	void Update()
	{
		if (timer > 0)
		{
			if (Mathf.CeilToInt(timer / blinkDuration) % 2 > 0)
			{
				cootsFront.material.SetFloat("_Cutoff", 1f);
				cootsBack.material.SetFloat("_Cutoff", 1f);
			}
			else
			{
				cootsFront.material.SetFloat("_Cutoff", 0.6f);
				cootsBack.material.SetFloat("_Cutoff", 0.6f);
			}
		}
		else
		{
			cootsFront.material.SetFloat("_Cutoff", 0.1f);
			cootsBack.material.SetFloat("_Cutoff", 0.1f);
		}

		timer -= Time.unscaledDeltaTime;
	}

	public void Blink()
	{
		timer = blinkDuration * blinkCount;
	}

	public void Unsheathe()
    {
        mr.material.mainTexture = unsheathed;
		Blink();
    }

    public void Sheathe()
    {
        mr.material.mainTexture = sheathed;
    }
}
