using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistancesController))]
public class Car : MonoBehaviour
{
    //[Header("Idle Kill Settings")]
    //[SerializeField] private float secondsToCheck;
    //[SerializeField] private float distanceToTravel;

    //[Header("Move Controls")]
    //[SerializeField] private float moveForwardMultiplier = 20;
    //[SerializeField] private float rotationMultiplier = 20;

    //[Header("Raycasts Controls")]
    //[SerializeField] private LayerMask raycastsMask;
    //[SerializeField] private float raycastLength = 1f;
    //[SerializeField] private int angleCoveredByRaycasts = 180;

    //public NeuralNetwork neuralNetwork = null;

    //private TrainingManager _trainingManager;
    //private WheelsController _wheelsController;
    //private DistancesController _distancesController;

    //private Rigidbody _rb;

    //private float currentTime;
    //private float previousDistance;

    //private bool inputsDisplayed;

    //private void Awake()
    //{
    //    _wheelsController = GetComponent<WheelsController>();
    //    _distancesController = GetComponent<DistancesController>();
    //    _trainingManager = FindObjectOfType<TrainingManager>();
    //    _rb = GetComponent<Rigidbody>();
    //}

    //private void FixedUpdate()
    //{
    //    var inputs = GetInputs();

    //    if(!inputsDisplayed)
    //    {
    //        Debug.Log($"Inputs: {string.Join(", ", inputs)}");
    //        inputsDisplayed = true;
    //    }

    //    (float accelerationMultiplier, float steerMultiplier) = neuralNetwork.Process(inputs);

    //    Move(accelerationMultiplier, steerMultiplier);

    //    KillIfIdle();
    //}

    //private double[] GetInputs()
    //{
    //    int angleCoveredByRaycasts = 180;
    //    int inputsCount = NeuralNetwork.Inputs;
    //    var inputs = new double[inputsCount];

    //    int angleBetweenRaycasts = angleCoveredByRaycasts / inputsCount;

    //    int raycastsCount = NeuralNetwork.Inputs - 1;

    //    inputs[0] = GetDistanceToWallFromRay(0);

    //    int inputsIndex = 1;

    //    for (int i = 1; i <= raycastsCount / 2; i++)
    //    {
    //        inputs[inputsIndex] = GetDistanceToWallFromRay(i * angleBetweenRaycasts);
    //        inputsIndex++;
    //        inputs[inputsIndex] = GetDistanceToWallFromRay(i * -angleBetweenRaycasts);
    //        inputsIndex++;
    //    }

    //    return inputs;
    //}

    //private double GetDistanceToWallFromRay(int rayRotation)
    //{
    //    int wallMask = 1 << 9;
    //    float raycastLength = 8f;

    //    var rotatedDirection = Quaternion.AngleAxis(rayRotation, transform.up) * transform.forward;

    //    RaycastHit hit;
    //    Physics.Raycast(transform.localPosition, rotatedDirection, out hit, raycastLength, wallMask);

    //    if (hit.collider == null)
    //    {
    //        return 1;
    //    }

    //    Debug.Log($"Hitted {hit.collider.tag}");

    //    return hit.distance / raycastLength;
    //}

    //public void InitialiseCar(NeuralNetwork neuralNetwork, Vector3 startPosition)
    //{
    //    var newPos = new Vector3(startPosition.x, transform.position.y, startPosition.z);
    //    transform.position = newPos;
    //    this.neuralNetwork = neuralNetwork;
    //}

    //private void Move(float accelerationMultiplier, float steerMultiplier)
    //{
    //    _rb.MovePosition(_rb.position + transform.forward * accelerationMultiplier * moveForwardMultiplier * Time.deltaTime);
    //    _rb.MoveRotation(Quaternion.Euler(_rb.rotation.eulerAngles + Vector3.up * steerMultiplier * rotationMultiplier * Time.deltaTime));

    //    //_wheelsController.Accelerate(accelerationMultiplier);
    //    //_wheelsController.SteerWheels(steerMultiplier);
    //}

    //public void DisableCar()
    //{
    //    float traveledDistance = _distancesController.GetTraveledDistance();
    //    _trainingManager.CarDeath(traveledDistance, transform.GetSiblingIndex());
    //}
    //private void KillIfIdle()
    //{
    //    if (currentTime == 0)
    //    {
    //        previousDistance = _distancesController.GetTraveledDistance();
    //    }

    //    currentTime += Time.deltaTime;

    //    if (currentTime < secondsToCheck) return;

    //    currentTime = 0;

    //    float currentDistance = _distancesController.GetTraveledDistance();
    //    float differenceBetweenDistances = Mathf.Abs(currentDistance - previousDistance);

    //    if (differenceBetweenDistances < distanceToTravel)
    //    {
    //        DisableCar();
    //    }
    //}
}
