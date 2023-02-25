using UnityEngine;

public class LeashPositionally : MonoBehaviour
{
    public Transform target;
    public float leashLength;

    void Update()
    {
        transform.position = Vector3.MoveTowards(target.position, transform.position, leashLength);
    }
}
