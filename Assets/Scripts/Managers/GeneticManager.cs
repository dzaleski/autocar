using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    private int initialPopulation;
    private float mutationProb;
    private int parentsCount;
    private int bestAgentSelection;
    private int worstAgentSelection;
    private int numberToCrossover;

    private NeuralNetwork[] population;

    private int inputs;
    private int neuronsPerHiddenLayer;
    private int hiddenLayers;

    private void Awake()
    {
        var trainingManager = FindObjectOfType<TrainingManager>();
        inputs = trainingManager.Inputs;
        hiddenLayers = trainingManager.HiddenLayers;
        neuronsPerHiddenLayer = trainingManager.NeuronsPerHiddenLayer;
        initialPopulation = trainingManager.initialPopulation;
        mutationProb = trainingManager.mutationRate;
        parentsCount = trainingManager.parentsCount;
        bestAgentSelection = trainingManager.bestAgentSelection;
        worstAgentSelection = trainingManager.worstAgentSelection;
        numberToCrossover = trainingManager.numberToCrossover;
    }
    public void CreateInitPopulation()
    {
        population = new NeuralNetwork[initialPopulation];

        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new NeuralNetwork(inputs, hiddenLayers, neuronsPerHiddenLayer);
        }
    }
    public NeuralNetwork[] GetCurrentPopulation()
    {
        return population;
    }

    public void Reproduce()
    {
        var newPopulation = new NeuralNetwork[population.Length];

        var parents = GetParentsAsBestNeuralNetworks();

        Debug.Log($"Population: {string.Join(", ", population.Select(x => x.fitness))}");
        Debug.Log($"Parents: {string.Join(", ", parents.Select(x => x.fitness))}");

        for (int i = 0; i < population.Length; i++)
        {
            newPopulation[i] = new NeuralNetwork(inputs, hiddenLayers, neuronsPerHiddenLayer, parents);
        }

        population = newPopulation; 
    }

    private List<NeuralNetwork> GetParentsAsBestNeuralNetworks()
    {
        return population.OrderByDescending(x => x.fitness).Take(parentsCount).ToList();
    }

    private List<NeuralNetwork> GetParentsFromGenePool()
    {
        var parents = new List<NeuralNetwork>();
        var parentsIndexes = new List<int>();

        var genePool = CreateGenePool();

        for (int i = 0; i < parentsCount; i++)
        {
            while (true)
            {
                int parentIndex = genePool[Random.Range(0, genePool.Count - 1)];

                if (!parentsIndexes.Contains(parentIndex))
                {
                    parentsIndexes.Add(parentIndex);
                    parents.Add(population[parentIndex]);
                    break;
                }
            }
        }

        return parents;
    }

    private List<int> CreateGenePool()
    {
        var genePool = new List<int>();

        float highestFitenss = population.Max(x => x.fitness);

        for (int i = 0; i < population.Length; i++)
        {
            float fitness = population[i].fitness.Remap(0, highestFitenss, 0f, 1f);
            int occursInGenePool = Mathf.FloorToInt(fitness * 100);

            for (int k = 0; k < occursInGenePool; k++)
            {
                genePool.Add(i);
            }
        }

        return genePool;
    }

    public void SetFitness(float fitness, int nnIndex)
    {
        population[nnIndex].fitness = fitness;
    }
}
