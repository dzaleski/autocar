using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GeneticManager
{
    public static int PopulationSize { get; private set; }
    private static float _mutationProb;
    private static int _parentsCount;

    private static NeuralNetwork bestNN;

    public static void Initialise(int populationSize, float mutationProb, int parentsCount)
    {
        PopulationSize = populationSize;
        _mutationProb = mutationProb;
        _parentsCount = parentsCount;

        bestNN = new NeuralNetwork();
        bestNN.Score = Mathf.NegativeInfinity;
    }

    public static NeuralNetwork[] GetFirstPopulation()
    {
        var firstPopulation = new NeuralNetwork[PopulationSize];

        for (int i = 0; i < PopulationSize; i++)
        {
            firstPopulation[i] = new NeuralNetwork();
        }

        return firstPopulation;
    }

    public static NeuralNetwork[] Reproduce(NeuralNetwork[] neuralNetworks)
    {
        var reproducedPopulation = new NeuralNetwork[PopulationSize];

        var bestFromPopulation = neuralNetworks.OrderByDescending(x => x.Score).First();

        if (bestFromPopulation.Score > bestNN.Score)
        {
            bestNN = new NeuralNetwork(bestFromPopulation);
            bestNN.Score = bestFromPopulation.Score;
        }

        var parents = neuralNetworks.OrderByDescending(x => x.Score).Take(_parentsCount).ToArray();

        reproducedPopulation[reproducedPopulation.Length - 1] = new NeuralNetwork(bestNN);

        for (int i = 0; i < reproducedPopulation.Length - 1; i++)
        {
            if(i < _parentsCount)
            {
                reproducedPopulation[i] = new NeuralNetwork(parents[i]);
            }
            else
            {
                reproducedPopulation[i] = new NeuralNetwork(parents);
            }
        }

        return reproducedPopulation;
    }

    private static NeuralNetwork[] GetParents(NeuralNetwork[] neuralNetworks)
    {
        var parents = new NeuralNetwork[_parentsCount];
        var genePool = CreateGenePoolFrom(neuralNetworks);

        var parentsIndexes = new List<int>();

        for (int i = 0; i < parents.Length; i++)
        {
            while (true)
            {
                int parentIndex = Random.Range(0, genePool.Count);

                if (!parentsIndexes.Contains(parentIndex))
                {
                    parentsIndexes.Add(parentIndex);
                    parents[i] = genePool[parentIndex];
                    break;
                }
            }
        }

        return parents;
    }

    private static List<NeuralNetwork> CreateGenePoolFrom(NeuralNetwork[] neuralNetworks)
    {
        var genePool = new List<NeuralNetwork>();

        float lowestScore = neuralNetworks.Min(x => x.Score);
        float highestScore = neuralNetworks.Max(x => x.Score);

        for (int i = 0; i < neuralNetworks.Length; i++)
        {
            int occursInGenePool = Mathf.RoundToInt(neuralNetworks[i].Score.Map(lowestScore, highestScore, 0f, 1f) * 100);

            for (int k = 0; k < occursInGenePool; k++)
            {
                genePool.Add(neuralNetworks[i]);
            }
        }

        return genePool;
    }

    public static void Mutate(NeuralNetwork[] population)
    {
        for (int i = 0; i < population.Length - 1; i++)
        {
            population[i].Mutate(_mutationProb);
        }
    }
}
