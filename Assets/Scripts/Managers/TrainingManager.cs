using System.Linq;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("Genetic Algorithm")]
    public int populationSize = 64;
    public float mutationProb = 0.055f;
    [Range(0, 20)] public int groupSize = 4;
    [Range(0, 1)] public float percentOfTheBestPass = 0.1f;
    [Range(0, 1f)] public float percentOfRandom = 0.1f;

    [Header("Neural Network")]
    public int hiddenLayers = 2;
    public int neuronsPerHiddenLayer = 12;

    [Header("Transforms")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform carsHolder;
    [SerializeField] private Transform parkedCarsParent;
    [SerializeField] private BoardGroup boardGroup;

    [Header("Prefabs")]
    [SerializeField] private AutoCar carPrefab;

    [Header("Controls")]
    [SerializeField] private int timeScale = 4;
    [SerializeField] private int mapsCount = 4;

    private ParkedCar[] parkedCars;

    private NeuralNetwork[][] neuralNetworksGroups;
    private AutoCar[] carsOfCurrentGroup;
    private int currentGroupIndex;
    private int currentPopulation;

    private void Awake()
    {
        GeneticManager.Initialise(populationSize, mutationProb, percentOfTheBestPass, percentOfRandom);
        NeuralNetwork.Initialise(hiddenLayers, neuronsPerHiddenLayer);
    }

    private void Start()
    {
        parkedCars = parkedCarsParent.GetComponentsInChildren<ParkedCar>();
        var countOfGroups = populationSize / groupSize;
        neuralNetworksGroups = new NeuralNetwork[countOfGroups][];

        var initNeuralNetworks = GeneticManager.GetInitialNetworks();

        FillNeuralNetworkGroupsFrom(initNeuralNetworks);
        InstantiateNextCarsGroup();
    }

    private void FillNeuralNetworkGroupsFrom(NeuralNetwork[] neuralNetworks)
    {
        for (int i = 0; i < neuralNetworksGroups.GetLength(0); i++)
        {
            neuralNetworksGroups[i] = neuralNetworks.Skip(i * groupSize).Take(groupSize).ToArray();
        }
    }

    private void Update()
    {
        Time.timeScale = timeScale;

        if (!DidCurrentGroupDie()) return;

        Debug.Log($"Current group: {currentGroupIndex}");
        Debug.Log($"Current population: {currentPopulation}");

        DestroyCurrentGroupCars();

        RestartParkedCars();

        if(currentGroupIndex >= neuralNetworksGroups.GetLength(0))
        {
            CreateNewPopulation();
            currentGroupIndex = 0;
            currentPopulation++;
        }

        InstantiateNextCarsGroup();
    }

    private bool DidCurrentGroupDie()
    {
        int diedBrainsCount = carsOfCurrentGroup.Count(x => x.Disabled);
        return diedBrainsCount == groupSize;
    }

    private void CreateNewPopulation()
    {
        var networks = neuralNetworksGroups.SelectMany(x => x).ToArray();
        var reproducedNetworks = GeneticManager.Reproduce(networks);
        GeneticManager.Mutate(reproducedNetworks);
        FillNeuralNetworkGroupsFrom(reproducedNetworks);
    }

    private void InstantiateNextCarsGroup()
    {
        var currentGroupNetworks = neuralNetworksGroups[currentGroupIndex];

        carsOfCurrentGroup = new AutoCar[currentGroupNetworks.Length];

        for (int i = 0; i < carsOfCurrentGroup.Length; i++)
        {
            carsOfCurrentGroup[i] = CreateBrain(currentGroupNetworks[i]);

        }

        currentGroupIndex++;
    }

    private AutoCar CreateBrain(NeuralNetwork neuralNetwork)
    {
        var car = Instantiate(carPrefab, carsHolder);
        car.SetNeuralNetwork(neuralNetwork);
        car.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        return car;
    }

    private void DestroyCurrentGroupCars()
    {
        for (int i = 0; i < carsOfCurrentGroup.Length; i++)
        {
            Destroy(carsOfCurrentGroup[i].gameObject);
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
