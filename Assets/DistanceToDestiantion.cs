using UnityEngine;
using UnityEngine.AI;

public class DistanceToDestiantion : MonoBehaviour
{
    private Vector3 destinationPoint;
    private Vector3 startPoint;

    private float distanceFromStartToDesitnation;
    public float DistanceFromStartToDest => distanceFromStartToDesitnation;

    void Awake()
    {
        destinationPoint = FindObjectOfType<Destination>().transform.position;
        startPoint = FindObjectOfType<StartPoint>().transform.position;
    }

    private void Start()
    {
        distanceFromStartToDesitnation = GetDistanceToDestinationPoint(startPoint);
    }

    public float GetDistanceToDestination()
    {
        return GetDistanceToDestinationPoint(transform.position);
    }

    private float GetDistanceToDestinationPoint(Vector3 from)
    {
        NavMeshPath path = new NavMeshPath();

        NavMesh.CalculatePath(from, destinationPoint, NavMesh.AllAreas, path);

        float wholeDistance = 0f;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            wholeDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return wholeDistance;
    }
}

