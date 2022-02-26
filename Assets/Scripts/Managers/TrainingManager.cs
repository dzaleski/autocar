using Assets.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingManager : MonoBehaviour
{
    [Header("GA Controls")]
    public int populationSize = 85;
    public float mutationProb = 0.055f;
    public int parentsCount = 2;

    [Header("NN Controls")]
    public int hiddenLayers = 1;
    public int neuronsPerHiddenLayer = 6;

    [Header("Transforms")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform carsHolder;

    [Header("Prefabs")]
    [SerializeField] private AutoCar carPrefab;

    [Header("Time Controls")]
    [SerializeField] private int timeScale = 1;

    private IEnumerable<AutoCar> cars;

    private void Awake()
    {
        GeneticManager.Initialise(populationSize, mutationProb, parentsCount);
        NeuralNetwork.Initialise(hiddenLayers, neuronsPerHiddenLayer);
        cars = GetBrainsFrom(GeneticManager.GetInitialNetworks());
    }

    private IEnumerable<AutoCar> GetBrainsFrom(IEnumerable<NeuralNetwork> neuralNetworks)
    {
        foreach (var network in neuralNetworks)
        {
            yield return CreateBrain(network);
        }
    }
    private AutoCar CreateBrain(NeuralNetwork neuralNetwork)
    {
        var car = Instantiate(carPrefab, carsHolder);
        car.SetNeuralNetwork(neuralNetwork);
        car.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        return car;
    }

    private void Start()
    {
        StartCoroutine(TrainingCoroutine());
    }

    private IEnumerator TrainingCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(DidWholePopulationDie);
            CreateNewPopulation();
        }
    }

    private bool DidWholePopulationDie()
    {
        int diedBrainsCount = cars.Count(x => x.Disabled);
        return diedBrainsCount >= populationSize;
    }

    private void CreateNewPopulation()
    {
        var networks = cars.Select(x => x.NeuralNetwork);
        var reproducedNetworks = GeneticManager.Reproduce(networks);

        DestroyAllBrains();

        GeneticManager.Mutate(reproducedNetworks);

        cars = GetBrainsFrom(reproducedNetworks);
    }

    private void DestroyAllBrains()
    {
        foreach (var brain in cars)
        {
            Destroy(brain.gameObject);
        }
    }

    private void Update()
    {
        Time.timeScale = timeScale;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyAllBrains();
            LoadMainMenu();
        }
    }

    private static void LoadMainMenu()
    {
        SceneManager.LoadScene((int)Scenes.MainMenu);
    }

    private void Restart()
    {
        DestroyAllBrains();
        cars = GetBrainsFrom(GeneticManager.GetInitialNetworks());
    }
}
