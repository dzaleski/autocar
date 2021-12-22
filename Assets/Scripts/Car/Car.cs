using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("NN Settings")]
    [SerializeField] private int hiddenLayers;
    [SerializeField] private int neuronsPerHiddenLayer;

    [Header("Idle Kill Settings")]
    [SerializeField] private float secondsToCheck;
    [SerializeField] private float distanceToTravel;

    [Header("Scoring")]
    [SerializeField] private float pointsForDistance;

    private WheelsController _wheelsController;
    private SensorsController _sensorController;
    private DistanceToDestiantion _distanceToDestiantion;

    private NeuralNetwork neuralNetwork;
    public NeuralNetwork NeuralNetwork
    { 
        get => neuralNetwork; 
        set => neuralNetwork = value;
    }

    private float carScore;
    public float CarScore => carScore;

    private float currentTime;
    private float previousDistance;

    private void Awake()
    {
        _wheelsController = GetComponent<WheelsController>();
        _sensorController = GetComponent<SensorsController>();
        _distanceToDestiantion = GetComponent<DistanceToDestiantion>();
    }

    private void Update()
    {
        float[] inputs = _sensorController.GetDistances();

        neuralNetwork.FeedForward(inputs);
        (float accelerationMultiplier, float steerMultiplier) = neuralNetwork.GetOutputs();

        Move(accelerationMultiplier, steerMultiplier);

        KillIfIdle();
    }

    private void Move(float accelerationMultiplier, float steerMultiplier)
    {
        _wheelsController.Accelerate(accelerationMultiplier);
        _wheelsController.SteerWheels(steerMultiplier);
    }

    private void KillIfIdle()
    {
        if (currentTime == 0)
        {
            previousDistance = _distanceToDestiantion.GetDistanceToDestination();
        }

        currentTime += Time.deltaTime;

        if (currentTime < secondsToCheck) return;

        currentTime = 0;

        float currentDistance = _distanceToDestiantion.GetDistanceToDestination();
        float differenceBetweenDistances = Mathf.Abs(currentDistance - previousDistance);

        if (differenceBetweenDistances < distanceToTravel)
        {
            DisableCar();
        }
    }

    public void DisableCar()
    {
        ChangePoints(-_distanceToDestiantion.GetDistanceToDestination());
        TrainingManager.Instance.CarDisabled();
        gameObject.SetActive(false);
    }

    public void ChangePoints(float amount)
    {
        neuralNetwork.Score += amount;
    }
}
