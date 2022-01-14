using UnityEngine;

public class ParkedCar : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRotation;


    private void Start()
    {
        startPos = transform.position;
        startRotation = transform.rotation;
    }

    public void RestartPosition()
    {
        transform.position = startPos;
        transform.rotation = startRotation;
    }
}
