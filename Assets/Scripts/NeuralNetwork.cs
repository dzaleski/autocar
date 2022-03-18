using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public class NeuralNetwork
{
    public float Fitness { get; set; }

    private static int _inputs;
    private static int _hiddenLayers;
    private static int _neuronsPerHiddenLayer;
    private static int _outputs;

    public List<float[,]> weightsBetweenTheLayers;
    private List<float[]> valuesOfNeurons;

    private int[] layerSizes;

    public NeuralNetwork()
    {
        layerSizes = new int[] { _inputs, _hiddenLayers, _neuronsPerHiddenLayer, _outputs };

        InitNeurons();
        InitWeights();
        FillWeightsWithRandomValues();
    }

    public NeuralNetwork(params NeuralNetwork[] parents) : this()
    {
        for (int i = 0; i < weightsBetweenTheLayers.Count; i++)
        {
            var currWeights = weightsBetweenTheLayers[i];

            for (int j = 0; j < currWeights.GetLength(0); j++)
            {
                for (int k = 0; k < currWeights.GetLength(1); k++)
                {
                    var randomParent = parents[Random.Range(0, parents.Length)];
                    var weightsFromRandomParent = randomParent.weightsBetweenTheLayers[i];
                    currWeights[j, k] = weightsFromRandomParent[j, k];
                }
            }
        }
    }

    private void InitNeurons()
    {
        valuesOfNeurons = new List<float[]>();

        foreach (var layerSize in layerSizes)
        {
            var layerNeurons = new float[layerSize];
            valuesOfNeurons.Add(layerNeurons);
        }
    }

    private void InitWeights()
    {
        weightsBetweenTheLayers = new List<float[,]>();

        for (int i = 1; i < layerSizes.Length; i++)
        {
            var previousLayerSize = layerSizes[i - 1];
            var currentLayerSize = layerSizes[i];
            var weightsMatrixBetweenLayers = new float[previousLayerSize, currentLayerSize];

            weightsBetweenTheLayers.Add(weightsMatrixBetweenLayers);
        }
    }

    private void FillWeightsWithRandomValues()
    {
        weightsBetweenTheLayers.ForEach(matrix => FillMatrixWithRandomValues(matrix));
    }

    private void FillMatrixWithRandomValues(float[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
            for (int k = 0; k < array.GetLength(1); k++)
                array[i, k] = GetRandomValue();
    }



    public NeuralNetwork(NeuralNetwork networkToCopy) : this()
    {
        weightsBetweenTheLayers = new List<float[,]>(networkToCopy.weightsBetweenTheLayers);
    }

    public static void Initialise(int inputs, int hiddenLayers, int neuronsPerHiddenLayer, int outputs)
    {
        _inputs = inputs;
        _outputs = outputs;
        _hiddenLayers = hiddenLayers;
        _neuronsPerHiddenLayer = neuronsPerHiddenLayer;
    }

    public float[] Process(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            valuesOfNeurons[0][i] = inputs[i]; //Fill first layer values with inputs
        }

        for (int currentLayer = 0; currentLayer < valuesOfNeurons.Count - 1; currentLayer++)
        {
            int currLayerSize = valuesOfNeurons[currentLayer].Length;
            var currNeurons = valuesOfNeurons[currentLayer];
            var currWeights = weightsBetweenTheLayers[currentLayer];

            int nextLayerIndex = currentLayer + 1;
            int nextLayerSize = valuesOfNeurons[nextLayerIndex].Length;

            for (int i = 0; i < nextLayerSize; i++)
            {
                float sum = 0;

                for (int j = 0; j < currLayerSize; j++)
                {
                    sum += currNeurons[j] * currWeights[j, i];
                }

                valuesOfNeurons[nextLayerIndex][i] = (float)Math.Tanh(sum);
            }
        }

        var outputLayer = valuesOfNeurons.Last();
        return outputLayer;
    }

    public void Mutate(float mutationProb)
    {
        for (int i = 0; i < weightsBetweenTheLayers.Count; i++)
        {
            for (int j = 0; j < weightsBetweenTheLayers[i].GetLength(0); j++)
            {
                if (!ShouldMutate(mutationProb))
                    continue;

                int randomCol = GetRandomValue(0, weightsBetweenTheLayers[i].GetLength(1));
                weightsBetweenTheLayers[i][j, randomCol] = GetRandomValue();
            }
        }
    }

    private bool ShouldMutate(float mutationProb)
    {
        float randomValueBetween01 = GetRandomValue(0f, 1f);

        if (randomValueBetween01 < mutationProb)
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

    private float ActivationFunction(float value)
    {
        //return (float)Math.Tanh(value);
        return value.Sigmoid();
    }

    public bool Equals(NeuralNetwork other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Fitness.Equals(other.Fitness) && weightsBetweenTheLayers.Equals(other.weightsBetweenTheLayers);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 13;
            hash = (hash * 7) + Fitness.GetHashCode();
            hash = (hash * 7) + weightsBetweenTheLayers.GetHashCode();
            return hash;
        }
    }
}