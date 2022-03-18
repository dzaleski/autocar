using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SensorsController : MonoBehaviour
{
    private static int _inputs = 8;
    private static int _length = 4;
    private static LayerMask _rayMask;
    private static bool _isVisible = true;

    [SerializeField] private BoxCollider boxCollider;

    private LineDrawer[] lineDrawers;

    private void Start()
    {
        lineDrawers = new LineDrawer[_inputs];

        for (int i = 0; i < lineDrawers.Length; i++)
        {
            lineDrawers[i] = new LineDrawer(transform);
        }
    }

    public static void Initialise(int inputs, int length, int rayMask, bool isVisible)
    {
        _inputs = inputs;
        _length = length;
        _rayMask = rayMask;
        _isVisible = isVisible;
    }

    public float[] GetInputs()
    {
        return GetDistances().ToArray();
    }

    private IEnumerable<float> GetDistances()
    {
        foreach (var (value, index) in GetOriginsWithDriections().WithIndexes())
        {
            var hitDistance = GetDistance(value.origin, value.direction);

            if (_isVisible)
            {
                DrawSensor(value.origin, value.direction, hitDistance, index);
            }

            yield return hitDistance;
        }
    }

    private IEnumerable<(Vector3 origin, Vector3 direction)> GetOriginsWithDriections()
    {
        var trans = boxCollider.transform;
        var center = boxCollider.center;
        var min = center - boxCollider.size * 0.5f;
        var max = center + boxCollider.size * 0.5f;

        yield return (trans.TransformPoint(new Vector3(min.x, max.y, min.z)), Quaternion.AngleAxis(45f, transform.up) * -transform.forward);
        yield return (trans.TransformPoint(new Vector3(center.x, max.y, min.z)), -transform.forward);
        yield return (trans.TransformPoint(new Vector3(max.x, max.y, min.z)), Quaternion.AngleAxis(-45f, transform.up) * -transform.forward);
        yield return (trans.TransformPoint(new Vector3(max.x, max.y, center.z)), transform.right);
        yield return (trans.TransformPoint(new Vector3(max.x, max.y, max.z)), Quaternion.AngleAxis(45f, transform.up) * transform.forward);
        yield return (trans.TransformPoint(new Vector3(center.x, max.y, max.z)), transform.forward);
        yield return (trans.TransformPoint(new Vector3(min.x, max.y, max.z)), Quaternion.AngleAxis(-45f, transform.up) * transform.forward);
        yield return (trans.TransformPoint(new Vector3(min.x, max.y, center.z)), -transform.right);
    }

    private float GetDistance(Vector3 origin, Vector3 direction)
    {
        float hitDistance = 1f;
        RaycastHit hit;

        Physics.Raycast(origin, direction, out hit, _length, _rayMask);

        if (hit.collider != null)
        {
            hitDistance = Mathf.Clamp01(hit.distance / _length);
        }

        return hitDistance;
    }

    private void DrawSensor(Vector3 origin, Vector3 direction, float distance, int index)
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

        lineDrawers[index].DrawLineInGameView(origin, origin + direction * _length, color);
    }
}