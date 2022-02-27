using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SensorsController : MonoBehaviour
{
    [Header("Raycasts Controls")]
    [SerializeField] private float length = 4f;
    [SerializeField] private bool isVisible = true;
    [SerializeField] private LayerMask rayMask;

    [Header("References")]
    [SerializeField] private BoxCollider boxCollider;

    private RaycastInfo[] raycastsInfo;

    private void Start()
    {
        raycastsInfo = new RaycastInfo[8];

        for (int i = 0; i < raycastsInfo.Length; i++)
        {
            raycastsInfo[i] = new RaycastInfo(transform);
        }
    }

    public IEnumerable<float> GetInputs()
    {
        UpdateRaycastsInfo();

        foreach (var raycastInfo in raycastsInfo)
        {
            var distance = GetDistance(raycastInfo);

            if (isVisible)
            {
                DrawRay(raycastInfo, distance);
            }

            yield return distance;
        }
    }

    private void UpdateRaycastsInfo()
    {
        var trans = boxCollider.transform;
        var center = boxCollider.center;
        var min = center - boxCollider.size * 0.5f;
        var max = center + boxCollider.size * 0.5f;


        raycastsInfo[0].UpdateRay(trans.TransformPoint(new Vector3(min.x, max.y, min.z)), Quaternion.AngleAxis(45f, transform.up) * -transform.forward);
        raycastsInfo[1].UpdateRay(trans.TransformPoint(new Vector3(max.x, max.y, min.z)), Quaternion.AngleAxis(-45f, transform.up) * -transform.forward);
        raycastsInfo[2].UpdateRay(trans.TransformPoint(new Vector3(max.x, max.y, max.z)), Quaternion.AngleAxis(45f, transform.up) * transform.forward);
        raycastsInfo[3].UpdateRay(trans.TransformPoint(new Vector3(min.x, max.y, max.z)), Quaternion.AngleAxis(-45f, transform.up) * transform.forward);

        raycastsInfo[4].UpdateRay(trans.TransformPoint(new Vector3(center.x, max.y, max.z)), transform.forward);
        raycastsInfo[5].UpdateRay(trans.TransformPoint(new Vector3(center.x, max.y, min.z)), -transform.forward);
        raycastsInfo[6].UpdateRay(trans.TransformPoint(new Vector3(max.x, max.y, center.z)), transform.right);
        raycastsInfo[7].UpdateRay(trans.TransformPoint(new Vector3(min.x, max.y, center.z)), -transform.right);
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

        raycastInfo.DrawRay(length, color);
    }

    public float GetDistance(RaycastInfo raycastInfo)
    {
        RaycastHit hit;

        Physics.Raycast(raycastInfo.Ray, out hit, length, rayMask);

        if (hit.collider == null)
        {
            return 1;
        }

        return Mathf.Clamp01(hit.distance / length);
    }
}

public class RaycastInfo
{
    public Ray Ray { get; private set; }

    private LineDrawer lineDrawer;

    public RaycastInfo(Transform transform)
    {
        Ray = new Ray();
        lineDrawer = new LineDrawer(transform);
    }

    public void UpdateRay(Vector3 origin, Vector3 direction)
    {
        Ray = new Ray(origin, direction);
    }

    public void DrawRay(float length, Color color)
    {
        lineDrawer.DrawLineInGameView(Ray.origin, Ray.origin + Ray.direction * length, color);
    }
}
