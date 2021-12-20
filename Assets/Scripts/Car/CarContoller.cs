using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CarContoller : MonoBehaviour
{
    [Header("Debug View")]
    [SerializeField] private float distanceToDestination;

    private WheelsController _wheelsController;
    private SensorsController _sensorController;
    private Car _car;
    private DistanceToDestiantion _distanceToDestiantion;

    private bool shouldUpdateDistance = true;

    private void Awake()
    {
        SetReferences();

        int inputsCount = _sensorController.Inputs;
        int countOfNeruonPerHiddenLayer = NNManager.Instance.countOfHiddenLayers;
        int countOfHiddenLayers = NNManager.Instance.countOfHiddenLayers;

        _car.neuralNetwork = new NeuralNetwork(inputsCount, countOfHiddenLayers, countOfNeruonPerHiddenLayer);
    }

    private void SetReferences()
    {
        _wheelsController = GetComponent<WheelsController>();
        _sensorController = GetComponent<SensorsController>();
        _car = GetComponent<Car>();
        _distanceToDestiantion = GetComponent<DistanceToDestiantion>();
    }

    private void Update()
    {
        float[] inputs = _sensorController.GetDistances();

        _car.neuralNetwork.FeedForward(inputs);
        (float accelerationMultiplier, float steerMultiplier) = _car.neuralNetwork.GetOutputs();

        Move(accelerationMultiplier, steerMultiplier);

        if (shouldUpdateDistance)
        {
            StartCoroutine(UpdateDistanceCoroutine());
        }
    }

    //private void GetDistanceToDestination()
    //{
    //    if(shouldUpdateDistance)
    //    {
    //        StartCoroutine(UpdateDistanceCoroutine());
    //    }
    //}

    private IEnumerator UpdateDistanceCoroutine()
    {
        distanceToDestination = _distanceToDestiantion.GetDistanceToDestination();
        shouldUpdateDistance = false;
        yield return new WaitForSeconds(2f);
        shouldUpdateDistance = true;
    }

    private void Move(float accelerationMultiplier, float steerMultiplier)
    {
        _wheelsController.Accelerate(accelerationMultiplier);
        _wheelsController.SteerWheels(steerMultiplier);
    }
}
