using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmAndResume : MonoBehaviour
{
    public GameObject toHide;

    public void DoIt()
    {
        toHide.SetActive(false);
        HitStop.instance.paused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
