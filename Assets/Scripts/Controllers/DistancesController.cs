using Assets.Scripts.Extensions;
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

    public float GetAvgDistanceBetweenVertices(BoxCollider from, BoxCollider to)
    {
        var fromVertices = from.GetVertices();
        var toVertices = to.GetVertices();

        float sumDistances = 0;
        int verticesCount = fromVertices.Length;

        for (int i = 0; i < verticesCount; i++)
        {
            sumDistances += Vector2.Distance(fromVertices[i].ToVector2XZ(), toVertices[i].ToVector2XZ());
        }

        return sumDistances / verticesCount;
    }

}

