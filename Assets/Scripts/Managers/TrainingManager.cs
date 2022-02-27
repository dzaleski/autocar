using Assets.Enums;
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
    }

    private void Start()
    {
        CreateInitialPopulation();
    }

    private void Update()
    {
        Time.timeScale = timeScale;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyCars();
            LoadMainMenu();
        }

        if(DidWholePopulationDie())
        {
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
        DestroyCars();
        GeneticManager.Mutate(reproducedNetworks);
        cars = GetBrainsFrom(reproducedNetworks).ToList();
    }

    private void CreateInitialPopulation()
    {
        var initNeuralNetworks = GeneticManager.GetInitialNetworks();
        cars = GetBrainsFrom(initNeuralNetworks).ToList();
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

    private void DestroyCars()
    {
        foreach (var car in cars)
        {
            Destroy(car.gameObject);
        }
    }

    private static void LoadMainMenu()
    {
        SceneManager.LoadScene((int)Scenes.MainMenu);
    }

    private void Restart()
    {
        DestroyCars();
        cars = GetBrainsFrom(GeneticManager.GetInitialNetworks());
    }
}
