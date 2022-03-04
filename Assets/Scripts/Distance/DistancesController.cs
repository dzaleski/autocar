using UnityEngine;
using UnityEngine.AI;

public class DistancesController: MonoBehaviour
{
    private Transform _transform;

    void Awake()
    {
        _transform = transform;
    }
    public float GetDistanceTo(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();

        NavMesh.CalculatePath(_transform.position, destination, NavMesh.AllAreas, path);

        float wholeDistance = 0f;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            wholeDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return wholeDistance;
    }

}

