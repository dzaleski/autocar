using System.Collections;
using System.Linq;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("GA Controls")]
    public int populationSize = 85;
    public float mutationProb = 0.055f;
    public int parentsCount = 2;

    [Header("NN Controls")]
    public int hiddenLayers = 1;
    public int neuronsPerHiddenLayer = 6;

    [Header("Positions")]
    [SerializeField] private Transform startPos;

    [Header("Holders")]
    [SerializeField] private Transform brainsHolder;

    [Header("Prefabs")]
    [SerializeField] private Brain brainPrefab;

    [Header("Simulation Settings")]
    [SerializeField] private int timeScale = 1;

    private UIManager uiManager;
    private ParkingController parkingController;

    private Brain[] brains;

    private void Awake()
    {
        parkingController = FindObjectOfType<ParkingController>();
        uiManager = FindObjectOfType<UIManager>();  
        GeneticManager.Initialise(populationSize, mutationProb, parentsCount);
        NeuralNetwork.Initialise(hiddenLayers, neuronsPerHiddenLayer);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CreateFirstPopulation();
        StartCoroutine(TrainingCoroutine());
        //StartCoroutine(EndGeneration());
    }

    private IEnumerator EndGeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);

            CreateNewPopulation();
        }
    }

    private void Update()
    {
        Time.timeScale = timeScale;
    }

    private void CreateFirstPopulation()
    {
        var firstPopulation = GeneticManager.GetFirstPopulation();
        CreateBrainsFrom(firstPopulation);
        uiManager.UpdateCurrentGenerationText();
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

    private IEnumerator TrainingCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(DidWholePopulationDie);

            CreateNewPopulation();
            uiManager.UpdateCurrentGenerationText();
        }
    }

    private Brain CreateBrain(NeuralNetwork neuralNetwork)
    {
        Brain brain = Instantiate(brainPrefab, brainsHolder);
        brain.Initialise(neuralNetwork, startPos.position);
        return brain;
    }

    private void CreateNewPopulation()
    {
        var NNs = brains.Select(x => x.neuralNetwork).ToArray();
        var reproducedNetworks = GeneticManager.Reproduce(NNs);
        
        DestroyAllBrains();

        GeneticManager.Mutate(reproducedNetworks);
        CreateBrainsFrom(reproducedNetworks);

        parkingController.RestartParkedCars();
    }

    private bool DidWholePopulationDie()
    {
        int diedBrainsCount = brains.Count(x => x.Disabled);
        return diedBrainsCount >= populationSize;
    }

    private void DestroyAllBrains()
    {
        for (int i = 0; i < brains.Length; i++)
        {
            Destroy(brains[i].gameObject);
        }
    }
}
