using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        //transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
    }
}
