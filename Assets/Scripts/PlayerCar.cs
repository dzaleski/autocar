using System.Linq;
using UnityEngine;

public class PlayerCar : Car
{
    [SerializeField] private Transform parkingSpot;

    private BoxCollider parkingSpotCollider;
    private float loss;
    private float startLossValue;

    private void Start()
    {
        parkingSpotCollider = parkingSpot.GetComponentInChildren<BoxCollider>();
    }

    private void Update()
    {
        sensorsController.GetInputs().ToList(); //In order to see raycasts
        loss = GetLoss();
        //SetLossText();
        MoveCar();
    }

    private void MoveCar() 
    { 
        wheelsController.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.Space) ? 1f : 0f);
    }

    public float GetLoss()
    {
        if (isFitnessDistance)
        {
            return distancesController.GetDistanceTo(parkingSpot.position);
        }

        return distancesController.GetAvgDistanceBetweenVertices(carCollider, parkingSpotCollider);
    }
}
