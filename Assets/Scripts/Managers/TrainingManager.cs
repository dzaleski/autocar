using System.Linq;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("Genetic Algorithm")]
    public int populationSize = 63;
    public float mutationProb = 0.055f;
    [Range(0, 20)] public int groupSize = 4;
    [Range(0, 1)] public float percentOfTheBestPass = 0.1f;
    [Range(0, 1f)] public float percentOfRandom = 0.1f;

    [Header("Neural Network")]
    public int hiddenLayers = 2;
    public int neuronsPerHiddenLayer = 12;

    [Header("Transforms")]
    [SerializeField] private BoardGroup boardGroup;

    [Header("Prefabs")]
    [SerializeField] private AutoCar carPrefab;

    [Header("Controls")]
    [SerializeField] private int timeScale = 4;
    [SerializeField] private int mapsCount = 4;

    private NeuralNetwork[][] neuralNetworksGroups;
    private int currentGroupIndex;
    private int currentPopulation;

    private void Awake()
    {
        GeneticManager.Initialise(populationSize, mutationProb, percentOfTheBestPass, percentOfRandom);
        NeuralNetwork.Initialise(hiddenLayers, neuronsPerHiddenLayer);
        groupSize = boardGroup.Boards.Count;
    }

    private void Start()
    {
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
        int diedCarsCount = boardGroup.Boards.Count(x => x.IsCarDisabled);
        return diedCarsCount == groupSize;
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

        for (int i = 0; i < currentGroupNetworks.Length; i++)
        {
            boardGroup.Boards[i].RestartParkedCars();
            boardGroup.Boards[i].InstantiateCar(currentGroupNetworks[i]);
        }

        currentGroupIndex++;
    }

    private void DestroyCurrentGroupCars()
    {
        foreach (var board in boardGroup.Boards)
        {
            board.DestroyCar();
        }
    }
}
