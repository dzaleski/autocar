using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform transformToFollow;
    [SerializeField] Vector3 offset;
    void Update()
    {
        transform.position = transformToFollow.position + offset;
        transform.LookAt(transformToFollow);
    }
}
