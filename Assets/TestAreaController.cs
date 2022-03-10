using UnityEngine;

public class TestAreaController : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform carsHolder;
    [SerializeField] private Transform parkedCarsParent;

    [Header("Prefabs")]
    [SerializeField] private AutoCar carPrefab;

    public AutoCar CreateBrain(NeuralNetwork neuralNetwork)
    {
        var car = Instantiate(carPrefab, carsHolder);
        car.SetNeuralNetwork(neuralNetwork);
        car.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        return car;
    }
}
