using System;
using System.Linq;
using Random = UnityEngine.Random;
using Mathf = UnityEngine.Mathf;

public static class GeneticManager
{
    private static int _populationSize;
    private static float _mutationProb;
    private static float _percentOfTheBestPass;
    private static int _parentsCount;
    private static float _percentOfRandom;

    public static void Initialise(int populationSize, float mutationProb, float percentOfTheBestPass, float percentOfRandom)
    {
        _populationSize = populationSize;
        _mutationProb = mutationProb;
        _percentOfTheBestPass = percentOfTheBestPass;
        _percentOfRandom = percentOfRandom;
    }

    public static NeuralNetwork[] GetInitialNetworks()
    {
        var initialNetworks = new NeuralNetwork[_populationSize];

        for (int i = 0; i < initialNetworks.Length; i++)
        {
            initialNetworks[i] = new NeuralNetwork();
        }

        return initialNetworks;
    }

    public static NeuralNetwork[] Reproduce(NeuralNetwork[] neuralNetworks)
    {
        var oldPopulation = new NeuralNetwork[_populationSize];
        var newPopulation = new NeuralNetwork[_populationSize];

        Array.Copy(neuralNetworks, oldPopulation, neuralNetworks.Length);

        var orderedNetworks = neuralNetworks.OrderByDescending(x => x.Fitness).ToArray(); //First network has best fitness

        var best = orderedNetworks.First().Fitness;

        int index = 0;

        _parentsCount = Mathf.FloorToInt(_percentOfTheBestPass * neuralNetworks.Length);

        while (index < _parentsCount)
        {
            newPopulation[index] = new NeuralNetwork(orderedNetworks[index]);
            index++;
        }

        var randomCount = Mathf.FloorToInt(_percentOfRandom * neuralNetworks.Length);

        while (index - _parentsCount < randomCount)
        {
            newPopulation[index] = new NeuralNetwork(orderedNetworks[index]);
            index++;
        }

        while (index < _populationSize)
        {
            int firstParentIndex = -1;
            NeuralNetwork firstParent = null;

            int secondParentIndex = -1;
            NeuralNetwork secondParent = null;

            while (firstParentIndex == secondParentIndex)
            {
                (firstParent, firstParentIndex) = GetWeightedRandomNetwork(orderedNetworks);
                (secondParent, secondParentIndex) = GetWeightedRandomNetwork(orderedNetworks);
            }

            var firstChildren = new NeuralNetwork(firstParent, secondParent);
            var secondChildren = new NeuralNetwork(firstParent, secondParent);

            newPopulation[index] = firstChildren;
            index++;

            if (index < _populationSize)
            {
                newPopulation[index] = secondChildren;
                index++;
            }
        }

        return newPopulation;
    }

    private static (NeuralNetwork, int) GetWeightedRandomNetwork(NeuralNetwork[] orderedNetworks)
    {
        // - weights = [1, 4, 3]
        // - cumulativeWeights = [1, 5, 8]
        var cumulativeWeights = new float[orderedNetworks.Length];
        for (int i = 0; i < orderedNetworks.Length; i += 1)
        {
            cumulativeWeights[i] = orderedNetworks[i].Fitness + (i != 0 ? cumulativeWeights[i - 1] : 0);
        }

        // Getting the random number in a range [0...sum(weights)]
        // For example:
        // - weights = [1, 4, 3]
        // - maxCumulativeWeight = 8
        // - range for the random number is [0...8]
        var maxCumulativeWeight = cumulativeWeights[cumulativeWeights.Length - 1];
        var randomNumber = Random.Range(0, maxCumulativeWeight);

        // Picking the random item based on its weight.
        // The items with higher weight will be picked more often.
        for (int i = 0; i < orderedNetworks.Length; i += 1)
        {
            if (cumulativeWeights[i] >= randomNumber)
            {
                return (orderedNetworks[i], i);
            }
        }

        return (orderedNetworks[orderedNetworks.Length - 1], orderedNetworks.Length - 1);
    }
    public static void Mutate(NeuralNetwork[] neuralNetworks)
    {
        for (int i = _parentsCount; i < neuralNetworks.Length; i++)
        {
            neuralNetworks[i].Mutate(_mutationProb);
        }
    }
}
