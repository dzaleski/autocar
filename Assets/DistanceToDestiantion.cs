using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DistanceToDestiantion : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private bool shouldDrawPath;
    [SerializeField] private float secondsToUpdatePath;

    private Transform target;
    private NavMeshPath path;

    void Start()
    {
        path = new NavMeshPath();
        target = FindObjectOfType<Destination>().transform;

        StartCoroutine(UpdatePathCoroutine());
    }
    private IEnumerator UpdatePathCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsToUpdatePath);
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        }
    }

    private void Update()
    {
        if (shouldDrawPath && path.corners.Length >= 1)
        {
            DrawPath();
        }
    }

    private void DrawPath()
    {
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
    }

    public float GetDistanceToDestination()
    {
        float wholeDistance = 0f;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            wholeDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return wholeDistance;
    }
}
