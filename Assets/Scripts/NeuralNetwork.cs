using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    [SerializeField] private int inputs;
    [SerializeField] private int countOfHiddenLayers;
    [SerializeField] private int countOfNeruonPerHiddenLayer;
    [SerializeField] private int outputs;
    [SerializeField] private float maxInitialValue;

    private const float EULER_NUMBER = 2.7182818f;

    private List<Neuron> inputLayer;
    private List<List<Neuron>> hiddenLayers;
    private List<Neuron> outputLayer;
    public NeuralNetwork()
    {
        InitInputLayer();        
        InitHiddenLayers();
        InitOutputLayer();
    }
    private void InitInputLayer()
    {
        inputLayer = new List<Neuron>();


        for (int i = 0; i < inputs; i++)
            inputLayer.Add(new Neuron());
    }
    private void InitHiddenLayers()
    {
        hiddenLayers = new List<List<Neuron>>();
        var firstHiddenLayer = new List<Neuron>();

        for (int i = 0; i < inputLayer.Count; i++)
            firstHiddenLayer.Add(new Neuron(inputLayer));

        hiddenLayers.Add(firstHiddenLayer);

        for (int i = 1; i < countOfHiddenLayers; i++)
        {
            var hiddenLayer = new List<Neuron>();

            for (int j = 0; j < countOfNeruonPerHiddenLayer; j++)
                hiddenLayer.Add(new Neuron(hiddenLayers[i - 1]));


            hiddenLayers.Add(hiddenLayer);
        }
    }
    private void InitOutputLayer()
    {
        outputLayer = new List<Neuron>();

        for (int i = 0; i < outputs; i++)
            outputLayer.Add(new Neuron(hiddenLayers.Last()));
    }

    public List<float> GetOutputs()
    {
        return outputLayer.Select(x => x.Value).ToList();
    }

    public float Sigmoid(float x)
    {
        float divider = 1 + Mathf.Pow(EULER_NUMBER, -x);
        return 1 / divider;
    }
}
