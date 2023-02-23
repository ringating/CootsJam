using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    public float speed = 180f;

    float z;

	private void Start()
	{
		z = 0;
	}

	void Update()
    {
		z = (360f + z + (speed * Time.deltaTime)) % 360f;
		transform.localRotation = Quaternion.Euler(0, 0, z);
    }
}
