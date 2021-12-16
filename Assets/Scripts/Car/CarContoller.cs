using UnityEngine;

public class CarContoller : MonoBehaviour
{
    [SerializeField] private WheelsController wheelsController;
    [SerializeField] private SensorsController sensorController;

    private NeuralNetwork neuralNetwork;

    private void Awake()
    {
        int inputsCount = sensorController.Inputs;
        int countOfNeruonPerHiddenLayer = NNManager.Instance.countOfHiddenLayers;
        int countOfHiddenLayers = NNManager.Instance.countOfHiddenLayers;

        neuralNetwork = new NeuralNetwork(inputsCount, countOfHiddenLayers, countOfNeruonPerHiddenLayer);
    }

    private void Update()
    {
        float[] inputs = sensorController.GetDistances();

        neuralNetwork.FeedForward(inputs);
        (float accelerationMultiplier, float steerMultiplier) = neuralNetwork.GetOutputs();

        Move(accelerationMultiplier, steerMultiplier);

        Debug.Log($"{accelerationMultiplier} | {steerMultiplier}");
    }

    private void Move(float accelerationMultiplier, float steerMultiplier)
    {
        wheelsController.Accelerate(accelerationMultiplier);
        wheelsController.SteerWheels(steerMultiplier);
    }


}
