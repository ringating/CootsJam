using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowImageWhenClose : MonoBehaviour
{
	public Image toShow;

	public float range = 4f;

	private void Start()
	{
		SetImageAlpha(toShow, 0);
	}

	private void Update()
	{
		if (Vector3.Distance(CootsController.instance.transform.position, transform.position) < range)
		{
			SetImageAlpha(toShow, Mathf.MoveTowards(toShow.color.a, 1, 2 * Time.deltaTime));
		}
		else
		{
			SetImageAlpha(toShow, Mathf.MoveTowards(toShow.color.a, 0, 2 * Time.deltaTime));
		}
	}

	public static void SetImageAlpha(Image image, float alpha)
	{
		Color c = image.color;
		c.a = alpha;
		image.color = c;
	}
}
