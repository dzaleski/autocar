using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GeneticManager
{
    public static int PopulationSize { get; private set; }
    private static float _mutationProb;
    private static int _parentsCount;

    public static void Initialise(int populationSize, float mutationProb, int parentsCount)
    {
        PopulationSize = populationSize;
        _mutationProb = mutationProb;
        _parentsCount = parentsCount;
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

    public static NeuralNetwork[] Reproduce(NeuralNetwork[] sortedNNs)
    {
        var reproducedPopulation = new NeuralNetwork[PopulationSize];

        var parents = sortedNNs.Take(_parentsCount).ToArray();

        for (int i = 0; i < reproducedPopulation.Length; i++)
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

    public static void Mutate(NeuralNetwork[] population)
    {
        for (int i = 0; i < population.Length; i++)
        {
            population[i].Mutate(_mutationProb);
        }
    }

    //private static List<NeuralNetwork> GetParentsAsBestNeuralNetworks()
    //{
    //    return population.OrderByDescending(x => x.fitness).Take(parentsCount).ToList();
    //}

    //private NeuralNetwork[] GetParentsFromGenePool(NeuralNetwork[] sortedNeuralNetworks)
    //{
    //    var parents = new NeuralNetwork[_parentsCount];
    //    var parentsIndexes = new List<int>();

    //    var genePool = CreateGenePool();

    //    for (int i = 0; i < parentsCount; i++)
    //    {
    //        while (true)
    //        {
    //            int parentIndex = genePool[Random.Range(0, genePool.Count - 1)];

    //            if (!parentsIndexes.Contains(parentIndex))
    //            {
    //                parentsIndexes.Add(parentIndex);
    //                parents.Add(population[parentIndex]);
    //                break;
    //            }
    //        }
    //    }

    //    return parents;
    //}

    //private static List<int> CreateGenePool()
    //{
    //    var genePool = new List<int>();

    //    float highestFitenss = population.Max(x => x.fitness);

    //    for (int i = 0; i < population.Length; i++)
    //    {
    //        float fitness = population[i].fitness.Remap(0, highestFitenss, 0f, 1f);
    //        int occursInGenePool = Mathf.FloorToInt(fitness * 100);

    //        for (int k = 0; k < occursInGenePool; k++)
    //        {
    //            genePool.Add(i);
    //        }
    //    }

    //    return genePool;
    //}

    //public void SetFitness(float fitness, int nnIndex)
    //{
    //    population[nnIndex].fitness = fitness;
    //}
}
