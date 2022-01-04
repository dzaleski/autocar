using UnityEngine;

public class Sensor : MonoBehaviour
{
    private float sensorLength = 50f;
    private float distanceToWall = 1;
    private Color rayColor;
    private bool shouldDrawRay;

    void Update()
    {
        CheckDistance();

        if(shouldDrawRay)
        {
            DrawRay();
        }
    }

    private void CheckDistance()
    {
        RaycastHit hit;
        var raycastDirection = transform.TransformDirection(Vector3.forward);
        Physics.Raycast(transform.position, raycastDirection, out hit, sensorLength);

        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            distanceToWall = hit.distance / sensorLength;
        }
        else
        {
            distanceToWall = 1;
        }
    }

    private void DrawRay()
    {
        var raycastDirection = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, raycastDirection * sensorLength, rayColor);
    }

    public float GetDistanceToWall()
    {
        return distanceToWall;
    }

    public void RototateByDegrees(int degrees) 
    {
        Vector3 localUpAxis = transform.TransformDirection(transform.up);
        transform.Rotate(localUpAxis, degrees);
    }

    public void MoveSensorForward(float howMuch)
    {
        transform.position = transform.position + transform.TransformDirection(Vector3.forward) * howMuch;
    }

    public void SetSensorColor(Color color)
    {
        rayColor = color;
    }

    public void SetSensorLength(int length)
    {
        sensorLength = length;
    }

    public void ShouldDrawRay(bool shouldRayBeVisible)
    {
        shouldDrawRay = shouldRayBeVisible;
    }
}
