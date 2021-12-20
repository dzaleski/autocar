using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralNetwork
{
    private int _inputs;
    private int _countOfHiddenLayers;
    private int _countOfNeruonPerHiddenLayer;
    private int _outputs;

    private const float EULER_NUMBER = 2.7182818f;

    private List<Neuron> inputLayer;
    private List<List<Neuron>> hiddenLayers;
    private List<Neuron> outputLayer;
    private List<List<Neuron>> allLayers;
    public NeuralNetwork(int inputs, int countOfHiddenLayes, int countOfNeruonPerHiddenLayer, int outputs = 2)
    {
        _inputs = inputs;
        _countOfHiddenLayers = countOfHiddenLayes;
        _countOfNeruonPerHiddenLayer = countOfNeruonPerHiddenLayer;
        _outputs = outputs;

        InitInputLayer();
        InitHiddenLayers();
        InitOutputLayer();
        InitAllLayers();
    }
    private void InitInputLayer()
    {
        inputLayer = new List<Neuron>();

        for (int i = 0; i < _inputs; i++)
        {
            inputLayer.Add(new Neuron());
        }
    }
    private void InitHiddenLayers()
    {
        hiddenLayers = new List<List<Neuron>>();

        List<Neuron> firstHiddenLayer = new List<Neuron>();

        for (int i = 0; i < _countOfNeruonPerHiddenLayer; i++)
        {
            Neuron hiddenLayerNeuron = new Neuron(inputLayer);
            firstHiddenLayer.Add(hiddenLayerNeuron);
        }

        hiddenLayers.Add(firstHiddenLayer);

        for (int i = 1; i < _countOfHiddenLayers; i++)
        {
            List<Neuron> previousHiddenLayer = hiddenLayers[i - 1];
            List<Neuron> hiddenLayer = new List<Neuron>();

            for (int j = 0; j < _countOfNeruonPerHiddenLayer; j++)
            {
                Neuron newNeuron = new Neuron(previousHiddenLayer);
                hiddenLayer.Add(newNeuron);
            }

            hiddenLayers.Add(hiddenLayer);
        }
    }
    private void InitOutputLayer()
    {
        outputLayer = new List<Neuron>();
        List<Neuron> lastHiddenLayer = hiddenLayers.Last();

        for (int i = 0; i < _outputs; i++)
        {
            Neuron newNeuron = new Neuron(lastHiddenLayer);
            outputLayer.Add(newNeuron);
        }
    }

    private void InitAllLayers()
    {
        allLayers = new List<List<Neuron>>();

        allLayers.Add(inputLayer);
        allLayers = allLayers.Union(hiddenLayers).ToList();
        allLayers.Add(outputLayer);
    }

    public void FeedForward(float[] inputs)
    {
        for (int i = 0; i < inputLayer.Count; i++)
        {
            Neuron neuronFromInputLayer = inputLayer[i];
            neuronFromInputLayer.Value = inputs[i];
        }

        for (int i = 1; i < allLayers.Count; i++)
        {
            List<Neuron> nextLayer = allLayers[i];

            foreach (Neuron neuron in nextLayer)
            {
                float sumOfProductValuesAndWeights = 0;

                foreach (Synapse synapse in neuron.incomeSynapses)
                {
                    Neuron neuronFromPreviousLayer = synapse.InputNeuron;
                    sumOfProductValuesAndWeights += neuronFromPreviousLayer.Value * synapse.Weight;
                }

                float valueFromActivationFunction = Tanh(sumOfProductValuesAndWeights);

                neuron.Value = valueFromActivationFunction;
            }
        }
    }

    public (float accelerationMultiplier, float steerMultiplier) GetOutputs()
    {
        return (outputLayer[0].Value, outputLayer[1].Value);
    }

    public float Tanh(float x)
    {
        float ePowX = Mathf.Pow(EULER_NUMBER, x);
        float ePowMinusX = Mathf.Pow(EULER_NUMBER, -x);

        float numerator = ePowX - ePowMinusX;
        float denominator = ePowX + ePowMinusX;

        return (numerator / denominator);
    }
}
