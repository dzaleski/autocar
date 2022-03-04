using UnityEngine;

public class Sensor
{
    private float _length;
    private int _rayMask;
    private bool _isVisible;
    private LineDrawer _lineDrawer;

    public Sensor(Transform parent, float length, int rayMask, bool isVisible = true)
    {
        _lineDrawer = new LineDrawer(parent);
        _rayMask = rayMask;
        _isVisible = isVisible;
        _length = length;
    }

    public float GetDistance(Vector3 origin, Vector3 direction)
    {
        float hitDistance = 1f;

        RaycastHit hit;

        Physics.Raycast(origin, direction, out hit, _length, _rayMask);

        if (hit.collider != null)
        {
            hitDistance = Mathf.Clamp01(hit.distance / _length);
        }

        if (_isVisible)
        {
            DrawSensor(origin, direction, hitDistance);
        }

        return hitDistance;
    }

    private void DrawSensor(Vector3 origin, Vector3 direction, float distance)
    {
        Color color = Color.green;

        if (distance <= 0.3f)
        {
            color = Color.red;
        }
        else if (distance < 1f)
        {
            color = Color.yellow;
        }

        _lineDrawer.DrawLineInGameView(origin, origin + direction * _length, color);
    }
}
