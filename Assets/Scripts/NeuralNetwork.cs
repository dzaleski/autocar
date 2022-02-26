using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class NeuralNetwork
{
    public float Score { get; set; }

    private static int _inputs;
    private static int _hiddenLayers;
    private static int _neuronsPerHiddenLayer;
    private static int _outputs;

    private List<double[,]> weights;
    private List<double[]> neurons;

    private int[] layerSizes;

    public NeuralNetwork()
    {
        layerSizes = new int[] { _inputs, _hiddenLayers, _neuronsPerHiddenLayer, _outputs };

        InitNeurons();
        InitWeights();
        FillWeightsWithRandomValues();
    }

    private void InitNeurons()
    {
        neurons = new List<double[]>();

        foreach (var layerSize in layerSizes)
        {
            var arrayOfNeurons = new double[layerSize];
            neurons.Add(arrayOfNeurons);
        }
    }

    private void InitWeights()
    {
        weights = new List<double[,]>();

        for (int i = 1; i < layerSizes.Length; i++)
        {
            var previousLayerSize = layerSizes[i - 1];
            var currentLayerSize = layerSizes[i];
            var weightsMatrixBetweenLayers = new double[previousLayerSize, currentLayerSize];

            weights.Add(weightsMatrixBetweenLayers);
        }
    }

    private void FillWeightsWithRandomValues()
    {
        weights.ForEach(matrix => FillMatrixWithRandomValues(matrix));
    }

    private void FillMatrixWithRandomValues(double[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
            for (int k = 0; k < array.GetLength(1); k++)
                array[i, k] = GetRandomValue();
    }

    public NeuralNetwork(IEnumerable<NeuralNetwork> parents) : this()
    {
        for (int i = 0; i < weights.Count; i++)
        {
            var currWeights = weights[i];

            for (int j = 0; j < currWeights.GetLength(0); j++)
            {
                for (int k = 0; k < currWeights.GetLength(1); k++)
                {
                    var randomParent = parents.GetRandomElement();
                    var weightsFromParent = randomParent.weights[i];
                    currWeights[j, k] = weightsFromParent[j, k];
                }
            }
        }
    }

    public NeuralNetwork(NeuralNetwork networkToCopy) : this()
    {
        weights = new List<double[,]>(networkToCopy.weights);
        Score = networkToCopy.Score;
    }

    public static void Initialise(int hiddenLayers, int neuronsPerHiddenLayer)
    {
        _inputs = 8; //Raycasts for all sides and all corners of car
        _outputs = 3; //To move forward nad backward, steer and brake
        _hiddenLayers = hiddenLayers;
        _neuronsPerHiddenLayer = neuronsPerHiddenLayer;
    }

    public double[] Process(IEnumerable<double> inputs)
    {
        foreach (var (input, index) in inputs.WithIndexes())
        {
            neurons[0][index] = input;
        }

        for (int currentLayer = 0; currentLayer < neurons.Count - 1; currentLayer++)
        {
            int currLayerSize = neurons[currentLayer].Length;
            var currNeurons = neurons[currentLayer];
            var currWeights = weights[currentLayer];

            int nextLayerIndex = currentLayer + 1;
            int nextLayerSize = neurons[nextLayerIndex].Length;

            for (int i = 0; i < nextLayerSize; i++)
            {
                double sum = 0;

                for (int j = 0; j < currLayerSize; j++)
                {
                    sum += currNeurons[j] * currWeights[j, i];
                }

                neurons[nextLayerIndex][i] = Math.Tanh(sum);
            }
        }

        var outputLayer = neurons.Last();

        return outputLayer;
    }
    public void Mutate(float mutationProb)
    {
        for (int i = 0; i < weights.Count; i++)
        {
            for (int j = 0; j < weights[i].GetLength(0); j++)
            {
                if (!ShouldMutate(mutationProb))
                    continue;

                int randomCol = GetRandomValue(0, weights[i].GetLength(1));
                weights[i][j, randomCol] = GetRandomValue();
            }
        }
    }

    private bool ShouldMutate(float mutationProb)
    {
        float randomValueBetween01 = GetRandomValue(0f, 1f);

        if (randomValueBetween01 > mutationProb)
        {
            return true;
        }

        return false;
    }

    private float GetRandomValue(float from, float to)
    {
        return Random.Range(from, to);
    }

    private int GetRandomValue(int from, int to)
    {
        return Random.Range(from, to);
    }

    private float GetRandomValue()
    {
        return Random.Range(-1f, 1f);
    }

    public bool Equals(NeuralNetwork other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Score.Equals(other.Score) && weights.Equals(other.weights);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 13;
            hash = (hash * 7) + Score.GetHashCode();
            hash = (hash * 7) + weights.GetHashCode();
            return hash;
        }
    }
}