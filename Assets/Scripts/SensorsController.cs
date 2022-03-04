using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SensorsController : MonoBehaviour
{
    [Header("Raycasts Controls")]
    [SerializeField] private float length = 4f;
    [SerializeField] private LayerMask rayMask;
    [SerializeField] private bool isVisible = true;

    [Header("References")]
    [SerializeField] private BoxCollider boxCollider;

    private Sensor[] sensors;

    private void Start()
    {
        sensors = new Sensor[8];

        for (int i = 0; i < sensors.Length; i++)
        {
            sensors[i] = new Sensor(transform, length, rayMask, isVisible);
        }
    }

    public float[] GetInputs()
    {
        return GetDistances().ToArray();
    }

    private IEnumerable<float> GetDistances()
    {
        var trans = boxCollider.transform;
        var center = boxCollider.center;
        var min = center - boxCollider.size * 0.5f;
        var max = center + boxCollider.size * 0.5f;

        yield return sensors[0].GetDistance(trans.TransformPoint(new Vector3(min.x, max.y, min.z)), Quaternion.AngleAxis(45f, transform.up) * -transform.forward);
        yield return sensors[1].GetDistance(trans.TransformPoint(new Vector3(max.x, max.y, min.z)), Quaternion.AngleAxis(-45f, transform.up) * -transform.forward);
        yield return sensors[2].GetDistance(trans.TransformPoint(new Vector3(max.x, max.y, max.z)), Quaternion.AngleAxis(45f, transform.up) * transform.forward);
        yield return sensors[3].GetDistance(trans.TransformPoint(new Vector3(min.x, max.y, max.z)), Quaternion.AngleAxis(-45f, transform.up) * transform.forward);
        yield return sensors[4].GetDistance(trans.TransformPoint(new Vector3(center.x, max.y, max.z)), transform.forward);
        yield return sensors[5].GetDistance(trans.TransformPoint(new Vector3(center.x, max.y, min.z)), -transform.forward);
        yield return sensors[6].GetDistance(trans.TransformPoint(new Vector3(max.x, max.y, center.z)), transform.right);
        yield return sensors[7].GetDistance(trans.TransformPoint(new Vector3(min.x, max.y, center.z)), -transform.right);
    }

    private IEnumerable<float> GetDistances2()
    {
        for (int i = 0; i < sensors.Length; i++)
        {
            yield return sensors[i].GetDistance(new Vector3(transform.position.x, 1.5f, transform.position.z), Quaternion.AngleAxis(45f * i, Vector3.up) * transform.forward);
        }
    }
}