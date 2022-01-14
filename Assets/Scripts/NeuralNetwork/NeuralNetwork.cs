using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class NeuralNetwork
{
    public float Score { get; set; }

    public static int Inputs { get; private set; }
    private static int _hiddenLayers;
    private static int _neuronsPerHiddenLayer;
    private static int _outputs;

    private List<double[,]> weights;
    private List<double[]> neurons;

    public NeuralNetwork()
    {
        neurons = new List<double[]>();
        weights = new List<double[,]>();

        InitWeightsAndNeurons();
    }

    private void InitWeightsAndNeurons()
    {
        var layerSizes = new int[] { Inputs, _hiddenLayers, _neuronsPerHiddenLayer, _outputs };

        for (int i = 0; i < layerSizes.Length; i++)
        {
            var layerNeuronValues = new double[layerSizes[i]];

            neurons.Add(layerNeuronValues);
        }

        for (int i = 1; i < layerSizes.Length; i++)
        {
            var prevToNextLayerWeights = new double[layerSizes[i - 1], layerSizes[i]];

            for (int j = 0; j < prevToNextLayerWeights.GetLength(0); j++)
            {
                for (int k = 0; k < prevToNextLayerWeights.GetLength(1); k++)
                {
                    prevToNextLayerWeights[j, k] = Random.Range(-1f, 1f);
                }
            }

            weights.Add(prevToNextLayerWeights);
        }
    }

    public NeuralNetwork(NeuralNetwork[] parents) : this()
    {
        for (int i = 0; i < weights.Count; i++)
        {
            for (int j = 0; j < weights[i].GetLength(0); j++)
            {
                for (int k = 0; k < weights[i].GetLength(1); k++)
                {
                    var weightsFromRandParent = GetWeightsFromRandomParent(parents);
                    weights[i][j, k] = weightsFromRandParent[i][j, k];
                }
            }
        }
    }

    public NeuralNetwork(NeuralNetwork networkToCopy) : this()
    {
        for (int i = 0; i < weights.Count; i++)
        {
            for (int j = 0; j < weights[i].GetLength(0); j++)
            {
                for (int k = 0; k < weights[i].GetLength(1); k++)
                {
                    weights[i][j, k] = networkToCopy.weights[i][j, k];
                }
            }
        }
    }

    private List<double[,]> GetWeightsFromRandomParent(NeuralNetwork[] parents)
    {
        return parents[Random.Range(0, parents.Length)].weights;
    }

    public static void Initialise(int hiddenLayers, int neuronsPerHiddenLayer)
    {
        Inputs = 8; //Raycast from every side and every corner of rectangular collider
        _hiddenLayers = hiddenLayers;
        _neuronsPerHiddenLayer = neuronsPerHiddenLayer;
        _outputs = 3; //First to move backward and forward, second to turn left and right, third to brake
    }

    public double[] Process(double[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
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

                float randomValueBetween01 = Random.Range(0f, 1f);

                if (randomValueBetween01 > mutationProb)
                {
                    continue;
                }

                int randomColumIndex = Random.Range(0, weights[i].GetLength(1));

                weights[i][j, randomColumIndex] = Random.Range(-1f, 1f);
            }
        }
    }
}