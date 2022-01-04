﻿using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("GA Controls")]
    public int initialPopulation = 85;
    [Range(0.0f, 1.0f)]
    public float mutationRate = 0.055f;
    public int parentsCount = 2;
    public int bestAgentSelection = 8;
    public int worstAgentSelection = 3;
    public int numberToCrossover;

    [Header("NN Settings")]
    public int Inputs;
    public int HiddenLayers;
    public int NeuronsPerHiddenLayer;

    [Header("Transforms")]
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private Transform carsHolder;

    [Header("Prefabs")]
    [SerializeField] private Car carPrefab;

    private GeneticManager geneticManager;
    private int disabledCarsCount;
    private int currentGeneration;

    private void Awake()
    {
        geneticManager = FindObjectOfType<GeneticManager>();
    }

    private void Start()
    {
        var neuralNetworks = geneticManager.GetCurrentPopulation();
        CreateCarsFromNeuralNetworks(neuralNetworks);
    }
    private void CreateCarsFromNeuralNetworks(NeuralNetwork[] neuralNetworks)
    {
        for (int i = 0; i < neuralNetworks.Length; i++)
        {
            CreateIndividualCar(neuralNetworks[i]);
        }

        currentGeneration = 1;
    }

    private Car CreateIndividualCar(NeuralNetwork neuralNetwork)
    {
        Car car = Instantiate(carPrefab, carsHolder);
        car.InitialiseCar(neuralNetwork, startPos.position);
        return car;
    }

    private void ReCreateCars()
    {
        currentGeneration++;
        DestroyAllCars();

        geneticManager.Reproduce();
        var neuralNetworks = geneticManager.GetCurrentPopulation();
        CreateCarsFromNeuralNetworks(neuralNetworks);

        Debug.Log(currentGeneration);
    }

    public void CarDeath(float fitness, int carIndex)
    {
        geneticManager.SetFitness(fitness, carIndex);

        disabledCarsCount++;

        Destroy(carsHolder.GetChild(carIndex).gameObject);

        if (disabledCarsCount == initialPopulation)
        {
            ReCreateCars();
            disabledCarsCount = 0;
        }
    }

    private void DestroyAllCars()
    {
        var cars = carsHolder.GetComponentsInChildren<Car>();

        for (int i = 0; i < cars.Length; i++)
        {
            Destroy(cars[i].gameObject);
        }
    }
}