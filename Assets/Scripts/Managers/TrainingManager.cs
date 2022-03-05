using System.Linq;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("Genetic Algorithm")]
    public int populationSize = 85;
    public float mutationProb = 0.055f;
    [Range(0, 1)] public float percentOfTheBestPass = 0.1f;
    [Range(0, .5f)] public float percentOfRandom = 0.05f;

    [Header("Neural Network")]
    public int hiddenLayers = 2;
    public int neuronsPerHiddenLayer = 12;

    [Header("Transforms")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform carsHolder;
    [SerializeField] private Transform parkedCarsParent;

    [Header("Prefabs")]
    [SerializeField] private AutoCar carPrefab;

    [Header("Time Controls")]
    [SerializeField] private int timeScale = 4;

    private AutoCar[] cars;
    private ParkedCar[] parkedCars;

    private void Awake()
    {
        GeneticManager.Initialise(populationSize, mutationProb, percentOfTheBestPass, percentOfRandom);
        NeuralNetwork.Initialise(hiddenLayers, neuronsPerHiddenLayer);
    }

    private void Start()
    {
        parkedCars = parkedCarsParent.GetComponentsInChildren<ParkedCar>();
        CreateInitialPopulation();
    }

    private void Update()
    {
        if (DidWholePopulationDie())
        {
            CreateNewPopulation();
        }

        Time.timeScale = timeScale;
    }

    private bool DidWholePopulationDie()
    {
        int diedBrainsCount = cars.Count(x => x.Disabled);
        return diedBrainsCount >= populationSize;
    }

    private void CreateNewPopulation()
    {
        var networks = cars.Select(x => x.NeuralNetwork).ToArray();

        var reproducedNetworks = GeneticManager.Reproduce(networks);

        GeneticManager.Mutate(reproducedNetworks);

        DestroyCars();

        cars = GetBrainsFrom(reproducedNetworks);

        RestartParkedCars();
    }

    private void CreateInitialPopulation()
    {
        var initNeuralNetworks = GeneticManager.GetInitialNetworks();
        cars = GetBrainsFrom(initNeuralNetworks);
    }

    private AutoCar[] GetBrainsFrom(NeuralNetwork[] neuralNetworks)
    {
        var cars = new AutoCar[neuralNetworks.Length];

        for (int i = 0; i < cars.Length; i++)
        {
            cars[i] = CreateBrain(neuralNetworks[i]);
        }

        return cars;
    }

    private AutoCar CreateBrain(NeuralNetwork neuralNetwork)
    {
        var car = Instantiate(carPrefab, carsHolder);
        car.SetNeuralNetwork(neuralNetwork);
        car.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        return car;
    }

    private void DestroyCars()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            Destroy(cars[i].gameObject);
        }
    }

    private void RestartParkedCars()
    {
        foreach (var car in parkedCars)
        {
            car.RestartPosition();
        }
    }

}
