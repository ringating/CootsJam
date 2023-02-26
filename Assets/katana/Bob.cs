using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{

	Vector3 localPos;

	private void Start()
	{
		localPos = transform.localPosition;
	}

	void Update()
    {
		transform.localPosition = localPos + (Vector3.up * 0.4f * Mathf.Sin(Time.time * Mathf.PI / 2f));
    }
}
