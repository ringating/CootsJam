using UnityEngine;

public class DropBridge : MonoBehaviour
{
	public Transform toDrop;
	
	bool bridgeDropped;

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("coots")) 
		{
			if (!bridgeDropped)
			{
				toDrop.transform.position = new Vector3(toDrop.transform.position.x, -10, toDrop.transform.position.z);
				bridgeDropped = true;
			}
		}
	}

	public void Undrop()
	{
		if (bridgeDropped)
		{
			toDrop.transform.position = new Vector3(toDrop.transform.position.x, -0.5f, toDrop.transform.position.z);
			bridgeDropped = false;
		}
	}
}
