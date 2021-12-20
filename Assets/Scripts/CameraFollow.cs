using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] private PopulationHolder populationHolder;

    void Update()
    {
        var transformToFollow = populationHolder.GetFirstCarTransform();
        transform.position = transformToFollow.position + offset;
        transform.LookAt(transformToFollow);
    }
}
