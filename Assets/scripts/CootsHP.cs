using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CootsHP : MonoBehaviour
{
    public static CootsHP instance;

	private void Start()
	{
        if (instance)
            Debug.LogError("there's more than 1 instance of CootsHP!");

        instance = this;
    }

	public float hp;
    public float[] powerLevelHP;

    public Transform hpBarParent;
    public Transform hpParent;

	private void Update()
	{
        UpdateMaxHP();
        UpdateHP();
    }

	public void UpdateMaxHP()
    {
        hpBarParent.localScale = new Vector3(powerLevelHP[CootsController.instance.powerLevel] / powerLevelHP[0], 1, 1);
    }

    public void UpdateHP()
    {
        hpParent.localScale = new Vector3(Mathf.Max(0, hp / powerLevelHP[CootsController.instance.powerLevel]), 1, 1);
    }

    public void DealDamage(float damage)
    {
        hp -= damage;
    }
}
