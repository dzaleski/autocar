using Assets.Scripts.Extensions;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private WheelsController wheelsController;
    [SerializeField] private SensorsController sensorsController;
    [SerializeField] private DistancesController distancesController;
    [SerializeField] private BoxCollider carCollider;
    [SerializeField] private TextMeshProUGUI lossText;

    private BoxCollider parkingSpotCollider;
    private Transform parkingSpotTransform;

    private void Awake()
    {
        var parkingSpotObject = GameObject.FindWithTag("ParkingSpot");
        parkingSpotCollider = parkingSpotObject.GetComponentInChildren<BoxCollider>();
        parkingSpotTransform = parkingSpotObject.transform;
    }

    private void Update()
    {
        sensorsController.GetInputs().ToList(); //In order to see raycasts
        MoveCar();
        SetLoss();
    }

    private void MoveCar()
    {
        wheelsController.SteerWheels(Input.GetAxis("Horizontal"));
        wheelsController.Accelerate(Input.GetAxis("Vertical"));
        wheelsController.Brake(Input.GetKey(KeyCode.Space) ? 1 : 0);
    }

    private void SetLoss()
    {
        var carVertices = carCollider.GetVertices();
        var parkingSportVertices = parkingSpotCollider.GetVertices();

        float distancesAvg = GetAvgDistanceBetweenVertices(carVertices, parkingSportVertices);

        SetLossText(distancesAvg);
    }

    private void SetLossText(float lossValue)
    {
        var roundedLoss = Math.Round(lossValue, 1);

        if(roundedLoss <= 1)
        {
            lossText.color = Color.green;
        }
        else if(roundedLoss < 3)
        {
            lossText.color = Color.yellow;
        }
        else
        {
            lossText.color = Color.red;
        }

        var distanceLeft = distancesController.GetDistanceTo(parkingSpotTransform.position) * 10f;

        var clampedLoss = Mathf.Clamp(400 - lossValue - distanceLeft, 0, 400);

        lossText.text = $"{Math.Round(clampedLoss, 1)}";
    }

    private float GetAvgDistanceBetweenVertices(Vector3[] fromVertices, Vector3[] toVertices)
    {
        float sumDistances = 0;
        int verticesCount = fromVertices.Length;

        for (int i = 0; i < verticesCount; i++)
        {
            sumDistances += Vector2.Distance(fromVertices[i].ToVector2XZ(), toVertices[i].ToVector2XZ()) * 10f;
        }

        return sumDistances / verticesCount;
    }
}
