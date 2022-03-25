using Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.AI;

public class DistancesController: MonoBehaviour
{
    private Transform carTransform;
    private BoxCollider boxCollider;
    private Transform parkingSpot;
    private BoxCollider parkingSpotCollider;

    void Awake()
    {
        carTransform = transform;
        boxCollider = GetComponentInChildren<BoxCollider>();
    }
    public float GetDistanceToParkingSpot()
    {
        NavMeshPath path = new NavMeshPath();

        NavMesh.CalculatePath(carTransform.position, parkingSpot.position, NavMesh.AllAreas, path);

        float wholeDistance = 0f;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            wholeDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return wholeDistance;
    }

    public float GetFitIntoParkingSpot()
    {
        var fromVertices = boxCollider.GetVertices();
        var toVertices = parkingSpotCollider.GetVertices();

        float sumDistances = 0;
        int verticesCount = fromVertices.Length;

        for (int i = 0; i < verticesCount; i++)
        {
            sumDistances += Vector2.Distance(fromVertices[i].ToVector2XZ(), toVertices[i].ToVector2XZ());
        }

        return sumDistances / verticesCount;
    }

    public void SetParkingSpotAndItsCollider(Transform spot)
    {
        parkingSpot = spot;
        parkingSpotCollider = spot.GetComponent<BoxCollider>();
    }
}

