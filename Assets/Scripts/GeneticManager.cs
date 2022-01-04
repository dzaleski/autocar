using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    private int initialPopulation;
    private float mutationRate;
    private int parentsCount;
    private int bestAgentSelection;
    private int worstAgentSelection;
    private int numberToCrossover;

    private List<int> genePool = new List<int>();

    private NeuralNetwork[] population;

    private int inputs;
    private int neuronsPerHiddenLayer;
    private int hiddenLayers;

    private int sumOfOccurs;

    private void Awake()
    {
        var trainingManager = FindObjectOfType<TrainingManager>();
        inputs = trainingManager.Inputs;
        hiddenLayers = trainingManager.HiddenLayers;
        neuronsPerHiddenLayer = trainingManager.NeuronsPerHiddenLayer;
        initialPopulation = trainingManager.initialPopulation;
        mutationRate = trainingManager.mutationRate;
        parentsCount = trainingManager.parentsCount;
        bestAgentSelection = trainingManager.bestAgentSelection;
        worstAgentSelection = trainingManager.worstAgentSelection;
        numberToCrossover = trainingManager.numberToCrossover;

        FillPopulation();
    }

    private void FillPopulation()
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
        genePool = new List<int>();

        float highestFitenss = GetHighestFitnessOfPopulation();

        for (int i = 0; i < population.Length; i++)
        {
            float fitness = population[i].fitness.Remap(0, highestFitenss, 0f, 1f);
            int occursInGenePool = Mathf.FloorToInt(fitness * 100);
            sumOfOccurs += occursInGenePool;
            for (int k = 0; k < occursInGenePool; k++)
            {
                genePool.Add(i);
            }
        }

        var newPopulation = new NeuralNetwork[population.Length];

        for (int i = 0; i < population.Length; i++)
        {
            var parents = GetParentsFromGenePool();
            newPopulation[i] = new NeuralNetwork(inputs, hiddenLayers, neuronsPerHiddenLayer, parents);
        }

        population = newPopulation;
    }

    private NeuralNetwork[] GetParentsFromGenePool()
    {
        var parents = new NeuralNetwork[2];
        var parentsIndexes = new int[2];

        for (int i = 0; i < parentsCount; i++)
        {
            for (int k = 0; k < 1000; k++)
            {
                int parentIndex = genePool[Random.Range(0, genePool.Count - 1)];

                if (!parentsIndexes.Contains(parentIndex))
                {
                    parents[i] = population[parentIndex];
                    break;
                }
            }
        }

        return parents;
    }

    private float GetHighestFitnessOfPopulation()
    {
        float result = 0;

        for (int i = 0; i < population.Length; i++)
        {
            if(population[i].fitness > result)
            {
                result = population[i].fitness;
            }
        }

        return result;
    }

    public void SetFitness(float fitness, int nnIndex)
    {
        population[nnIndex].fitness = fitness;
    }
}
