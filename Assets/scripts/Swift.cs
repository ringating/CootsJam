using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swift : MonoBehaviour
{
	public static Swift instance;

    public MeshRenderer dialogue0;
    public MeshRenderer dialogue1;
    public MeshRenderer dialogue2;

	private void Start()
	{
		instance = this;
	}

	public void SetDialogue(int dialogueIndex)
	{
		dialogue0.enabled = false;
		dialogue1.enabled = false;
		dialogue2.enabled = false;

		switch (dialogueIndex) 
		{
			case 0:
				dialogue0.enabled = true;
				break;

			case 1:
				dialogue1.enabled = true;
				break;

			case 2:
				dialogue2.enabled = true;
				break;
		}
	}
}
