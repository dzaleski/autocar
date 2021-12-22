using System.Linq;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("GA Settings")]
    [SerializeField] private int populationSize;
    [SerializeField] private float mutationProb;
    [SerializeField] private int numberOfParents;

    [Header("NN Settings")]
    [SerializeField] public int Inputs;
    [SerializeField] public int HiddenLayers;
    [SerializeField] public int NeuronsPerHiddenLayer;

    [Header("Transforms")]
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private Transform carsHolder;

    [Header("Prefabs")]
    [SerializeField] private Car carPrefab;

    private GeneticAlgorithm geneticAlgorithm;
    private Car[] cars;
    private int disabledCarsCount;
    private int currentGeneration;

    private static TrainingManager instance;
    public static TrainingManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        geneticAlgorithm = new GeneticAlgorithm(populationSize, mutationProb, numberOfParents);
    }

    private void Start()
    {
        CreateInitialPopulation();
    }

    private void CreateInitialPopulation()
    {
        currentGeneration = 1;
        NeuralNetwork[] neuralNetworks = geneticAlgorithm.CreateInitialPopulation();
        CreatePopulationFromNetworks(neuralNetworks);
    }

    private void CreatePopulationFromNetworks(NeuralNetwork[] networks)
    {
        cars = new Car[networks.Length];

        for (int i = 0; i < networks.Length; i++)
        {
            cars[i] = CreateIndividualCar(networks[i]);
        }
    }

    private Car CreateIndividualCar(NeuralNetwork neuralNetwork)
    {
        Car car = Instantiate(carPrefab, carsHolder);
        car.transform.position = startPos.position;
        car.NeuralNetwork = neuralNetwork;

        return car;
    }

    public void CarDisabled()
    {
        disabledCarsCount++;

        if(disabledCarsCount == cars.Length)
        {
            Reproduce();
            disabledCarsCount = 0;
        }
    }

    private void Reproduce()
    {
        currentGeneration++;

        NeuralNetwork[] neuralNetworks = geneticAlgorithm.Reproduce();

        DestroyAllCars();

        CreatePopulationFromNetworks(neuralNetworks);

        Debug.Log("Current Generation " + currentGeneration);
    }

    private void DestroyAllCars()
    {
        for (int i = 0;i < cars.Length;i++)
        {
            Destroy(cars[i].gameObject);
        }

        cars = new Car[populationSize];
    }
}
