using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    public static Death instance;
    public Image blackScreen;
    public BlackScreen blackScreenScript;
    public Animator blackScreenAnimator;
    public Transform postFirstBossRespawnPoint;
    public HandAnimationStuff handAnimStuff;

    public DropBridge finalBossBridge;

    public delegate void RespawnIngester();
    public event RespawnIngester OnRespawn;

    public PowerPickup gunPower;
    public PowerPickup katanaPower;

    public void TriggerRespawnEvents()
    {
        OnRespawn?.Invoke();
    }

    bool died = false;

    List<Hurtable> respawnable;

	private void Start()
	{
        instance = this;

        OnRespawn += finalBossBridge.Undrop;

        Hurtable[] hurtables = FindObjectsOfType<Hurtable>();
        respawnable = new List<Hurtable>();
        foreach (Hurtable h in hurtables) 
        {
            if (!h.CompareTag("coots") && !h.CompareTag("vase"))
                respawnable.Add(h);
        }
    }

	void Update()
    {
        if (!died && CootsHP.instance.hp <= 0)
        {
            died = true;

            if (!Flags.encounteredFirstBoss)
            {
                //  lmao
                SceneManager.LoadScene(0);
            }
            else if (!Flags.diedToFirstBoss)
            {
                Flags.diedToFirstBoss = true;
                Swift.instance.SetDialogue(0);
                handAnimStuff.Hide();

                Respawn(postFirstBossRespawnPoint);
            }
            else
            {
                if (!Flags.defeatedGunBoss)
                {
                    CootsController.instance.hasGun = false;
                    gunPower.gameObject.SetActive(true);
                }

                if (!Flags.defeatedKatanaBoss)
                {
                    CootsController.instance.hasKatana = false;
                    katanaPower.gameObject.SetActive(true);
                }
            }
        }
    }

    public void Respawn(Transform respawnPoint)
    {
        blackScreenScript.respawnPoint = respawnPoint;
        blackScreenAnimator.CrossFade("respawn black screen fade", 0);
    }
}
