using System.Linq;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("GA Controls")]
    public int populationSize = 85;
    public float mutationProb = 0.055f;
    public int parentsCount = 2;

    [Header("NN Controls")]
    public int inputs = 5;
    public int hiddenLayers = 1;
    public int neuronsPerHiddenLayer = 6;

    [Header("Positions")]
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    [Header("Holders")]
    [SerializeField] private Transform brainsHolder;

    [Header("Prefabs")]
    [SerializeField] private Brain brainPrefab;

    [Header("Simulation Settings")]
    [SerializeField] private int timeScale = 1;

    private int disabledCarsCount;
    private int currentGeneration = 1;

    private Brain[] brains;

    private void Awake()
    {
        GeneticManager.Initialise(populationSize, mutationProb, parentsCount);
        NeuralNetwork.Initialise(inputs, hiddenLayers, neuronsPerHiddenLayer, 2);
        CreateFirstPopulation();
    }

    private void CreateFirstPopulation()
    {
        var firstPopulation = GeneticManager.GetFirstPopulation();
        CreateBrainsFrom(firstPopulation);
    }

    private void CreateBrainsFrom(NeuralNetwork[] neuralNetworks)
    {
        brains = new Brain[populationSize];

        for (int i = 0; i < neuralNetworks.Length; i++)
        {
            var brain = CreateBrain(neuralNetworks[i]);
            brains[i] = brain;
        }
    }

    private void Update()
    {
        Time.timeScale = timeScale;

        if (IsWholePopulationDied())
        {
            currentGeneration++;
            Debug.Log($"Current Generation: {currentGeneration}");
            ReCreateCars();
        }
    }

    private Brain CreateBrain(NeuralNetwork neuralNetwork)
    {
        Brain brain = Instantiate(brainPrefab, brainsHolder);
        brain.Initialise(neuralNetwork, startPos.position);
        return brain;
    }

    private void ReCreateCars()
    {
        var sortedNNs = brains.OrderByDescending(x => x.Score).Select(x => x.neuralNetwork).ToArray();
        var reproducedNetworks = GeneticManager.Reproduce(sortedNNs);

        DestroyAllBrains();

        GeneticManager.Mutate(reproducedNetworks);
        CreateBrainsFrom(reproducedNetworks);
    }

    private bool IsWholePopulationDied()
    {
        int diedBrainsCount = brains.Count(x => x.Disabled);
        return diedBrainsCount == populationSize;
    }

    private void DestroyAllBrains()
    {
        for (int i = 0; i < brains.Length; i++)
        {
            Destroy(brains[i].gameObject);
        }
    }
}
