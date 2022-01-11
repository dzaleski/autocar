using UnityEngine;
using UnityEngine.AI;

public class DistancesController: MonoBehaviour
{
    public Vector3 Destination { get; private set; }
    public Vector3 Start { get; private set; }

    void Awake()
    {
        Destination = FindObjectOfType<Destination>().transform.position;
        Start = FindObjectOfType<StartPoint>().transform.position;
    }

    public float GetTraveledDistance()
    {
        return GetDistanceFromTo(Start, transform.position);
    }

    public float GetDistanceFromStartToDestination()
    {
        return GetDistanceFromTo(Start, Destination);
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

