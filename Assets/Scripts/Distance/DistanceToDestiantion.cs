using UnityEngine;
using UnityEngine.AI;

public class DistanceToDestiantion : MonoBehaviour
{
    private Vector3 destinationPoint;
    private Vector3 startPoint;

    void Awake()
    {
        destinationPoint = FindObjectOfType<Destination>().transform.position;
        startPoint = FindObjectOfType<StartPoint>().transform.position;
    }
    public float GetTraveledDistance()
    {
        return GetDistanceFromTo(startPoint, transform.position);
    }

    public float GetDistanceFromStartToDestination()
    {
        return GetDistanceFromTo(startPoint, destinationPoint);
    }

    private float GetDistanceFromTo(Vector3 from, Vector3 to)
    {
        NavMeshPath path = new NavMeshPath();

        NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);

        float wholeDistance = 0f;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            wholeDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return wholeDistance;
    }

}

