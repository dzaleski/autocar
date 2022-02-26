using System.Collections.Generic;
using UnityEngine;

public class SensorsController : MonoBehaviour
{
    [Header("Raycasts Controls")]
    [SerializeField] private float length = 4f;
    [SerializeField] private float upOffset = 1.2f;
    [SerializeField] private bool isVisible = true;
    [SerializeField] private LayerMask rayMask;

    [Header("References")]
    [SerializeField] private BoxCollider boxCollider;

    public IEnumerable<double> GetInputs()
    {
        foreach (var raycastInfo in GetRaycastsInfo())
        {
            var distance = GetDistance(raycastInfo);

            if (isVisible)
            {
                DrawRay(raycastInfo, distance);
            }

            yield return distance;
        }
    }

    private IEnumerable<RaycastInfo> GetRaycastsInfo()
    {
        var trans = boxCollider.transform;
        var center = boxCollider.center;
        var min = center - boxCollider.size * 0.5f;
        var max = center + boxCollider.size * 0.5f;

        yield return new RaycastInfo(trans.TransformPoint(new Vector3(min.x, max.y, min.z)), Quaternion.AngleAxis(45f, transform.up) * -transform.forward);
        yield return new RaycastInfo(trans.TransformPoint(new Vector3(max.x, max.y, min.z)), Quaternion.AngleAxis(-45f, transform.up) * -transform.forward);
        yield return new RaycastInfo(trans.TransformPoint(new Vector3(max.x, max.y, max.z)), Quaternion.AngleAxis(45f, transform.up) * transform.forward);
        yield return new RaycastInfo(trans.TransformPoint(new Vector3(min.x, max.y, max.z)), Quaternion.AngleAxis(-45f, transform.up) * transform.forward);

        yield return new RaycastInfo(trans.TransformPoint(new Vector3(center.x, max.y, max.z)), transform.forward);
        yield return new RaycastInfo(trans.TransformPoint(new Vector3(center.x, max.y, min.z)), -transform.forward);
        yield return new RaycastInfo(trans.TransformPoint(new Vector3(max.x, max.y, center.z)), transform.right);
        yield return new RaycastInfo(trans.TransformPoint(new Vector3(min.x, max.y, center.z)), -transform.right);
    }

    private void DrawRay(RaycastInfo raycastInfo, double distance)
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

        Debug.DrawRay(raycastInfo.Origin, raycastInfo.Direction * length, color);
    }

    public double GetDistance(RaycastInfo raycastInfo)
    {
        RaycastHit hit;

        Physics.Raycast(raycastInfo.Origin, raycastInfo.Direction, out hit, length, rayMask);

        if (hit.collider == null)
        {
            return 1;
        }

        return Mathf.Clamp01(hit.distance / length);
    }
}

public struct RaycastInfo
{
    public RaycastInfo(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Vector3 Origin { get; set; }
    public Vector3 Direction { get; set; }
}
