using UnityEngine;

public class Sensor : MonoBehaviour
{
    private float sensorLength = 50f;
    private Color rayColor;
    private bool shouldDrawRay;

    void Update()
    {
        if(shouldDrawRay)
        {
            DrawRay();
        }
    }
    private void DrawRay()
    {
        var raycastDirection = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, raycastDirection * sensorLength, rayColor);
    }

    public double GetDistanceToWall()
    {
        RaycastHit hit;
        var raycastDirection = transform.TransformDirection(Vector3.forward);
        Physics.Raycast(transform.position, raycastDirection, out hit, sensorLength);

        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            return Mathf.Clamp01(hit.distance / sensorLength);
        }

        return 1;
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
