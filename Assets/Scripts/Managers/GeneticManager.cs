using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GeneticManager
{
    public static NeuralNetwork BestNN { get; private set; }

    public static int PopulationSize { get; private set; }
    private static float _mutationProb;
    private static int _parentsCount;
    public static void Initialise(int populationSize, float mutationProb, int parentsCount)
    {
        PopulationSize = populationSize;
        _mutationProb = mutationProb;
        _parentsCount = parentsCount;

        BestNN = new NeuralNetwork();
        BestNN.Score = Mathf.NegativeInfinity;
    }

    public static IEnumerable<NeuralNetwork> GetInitialNetworks()
    {
        for (int i = 0; i < PopulationSize; i++)
        {
            yield return new NeuralNetwork();
        }
    }

    public static IEnumerable<NeuralNetwork> Reproduce(IEnumerable<NeuralNetwork> neuralNetworks)
    {
        var networksCount = neuralNetworks.Count();
        var bestFromPopulation = neuralNetworks.OrderByDescending(x => x.Score).First();

        if (bestFromPopulation.Score > BestNN.Score)
        {
            BestNN = new NeuralNetwork(bestFromPopulation);
        }

        var parents = neuralNetworks.OrderByDescending(x => x.Score).Take(_parentsCount).ToArray();

        for (int i = 0; i < networksCount - 1; i++)
        {
            yield return new NeuralNetwork(parents);
        }

        yield return new NeuralNetwork(BestNN);
    }

    private static NeuralNetwork[] GetParents(NeuralNetwork[] neuralNetworks)
    {
        var parents = new NeuralNetwork[_parentsCount];
        var genePool = CreateGenePoolFrom(neuralNetworks);

        //var a = genePool.Select(x => new { Score = x.Score, Occurs = genePool.Count(y => x == y) }).Select(x => $"Score: {x.Score} | Occurs: {x.Occurs}");

        //foreach (var item in a)
        //{
        //    Debug.Log(item);
        //}

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

    public static void Mutate(IEnumerable<NeuralNetwork> neuralNetworks)
    {
        foreach (var network in neuralNetworks)
        {
            network.Mutate(_mutationProb);
        }
    }
}
