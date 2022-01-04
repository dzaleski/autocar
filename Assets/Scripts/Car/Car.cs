using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Idle Kill Settings")]
    [SerializeField] private float secondsToCheck;
    [SerializeField] private float distanceToTravel;

    [NonSerialized] public NeuralNetwork neuralNetwork;

    private TrainingManager _trainingManager;
    private WheelsController _wheelsController;
    private SensorsController _sensorsController;
    private DistanceToDestiantion _distanceToDestiantion;

    private float currentTime;
    private float previousDistance;

    private void Awake()
    {
        _wheelsController = GetComponent<WheelsController>();
        _sensorsController = GetComponent<SensorsController>();
        _distanceToDestiantion = GetComponent<DistanceToDestiantion>();
        _trainingManager = FindObjectOfType<TrainingManager>();
    }

    private void Update()
    {
        float[] inputs = _sensorsController.GetDistances();

        (float accelerationMultiplier, float steerMultiplier) = neuralNetwork.Process(inputs);

        Move(accelerationMultiplier, steerMultiplier);

        KillIfIdle();
    }

    public void InitialiseCar(NeuralNetwork neuralNetwork, Vector3 startPosition)
    {
        transform.position = startPosition;
        this.neuralNetwork = neuralNetwork;
    }

    private void Move(float accelerationMultiplier, float steerMultiplier)
    {
        _wheelsController.Accelerate(accelerationMultiplier);
        _wheelsController.SteerWheels(steerMultiplier);
    }

    public void DisableCar()
    {
        float wholeDistnace = _distanceToDestiantion.GetDistanceFromStartToDestination();
        float traveledDistance = _distanceToDestiantion.GetTraveledDistance();
        float fitness = Mathf.Clamp01(traveledDistance / wholeDistnace);

        _trainingManager.CarDeath(fitness, transform.GetSiblingIndex());
    }
    private void KillIfIdle()
    {
        if (currentTime == 0)
        {
            previousDistance = _distanceToDestiantion.GetTraveledDistance();
        }

        currentTime += Time.deltaTime;

        if (currentTime < secondsToCheck) return;

        currentTime = 0;

        float currentDistance = _distanceToDestiantion.GetTraveledDistance();
        float differenceBetweenDistances = Mathf.Abs(currentDistance - previousDistance);

        if (differenceBetweenDistances < distanceToTravel)
        {
            DisableCar();
        }
    }
}
