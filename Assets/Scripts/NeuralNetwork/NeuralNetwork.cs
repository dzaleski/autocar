using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

public class NeuralNetwork
{
    private int _inputsCount;
    private int _neuronsPerHiddenLayer;
    private int _hiddenLayers;
    private int _outputsCount;

    private List<Matrix<float>> _weightsList;
    private List<Matrix<float>> _neuronsValuesList;
    private List<Matrix<float>> _biasesList;

    public List<Matrix<float>> WeightsList => _weightsList;
    public List<Matrix<float>> BiasesList => _biasesList;

    public float fitness;

    public NeuralNetwork(int inputs, int hiddenLayers, int neuronsPerHiddenLayer)
    {
        _inputsCount = inputs;
        _neuronsPerHiddenLayer = neuronsPerHiddenLayer;
        _hiddenLayers = hiddenLayers;
        _outputsCount = 2;
        _neuronsValuesList = new List<Matrix<float>>();
        _weightsList = new List<Matrix<float>>();
        _biasesList = new List<Matrix<float>>();

        FIllBiasesWithRandomValues();
        FillRandomNeuronValues();
        FillWeightsWithRandomValues();
    }
    
    public NeuralNetwork(int inputs, int hiddenLayers, int neuronsPerHiddenLayer, NeuralNetwork[] parents) : this(inputs, hiddenLayers, neuronsPerHiddenLayer)
    {
        for(int i = 0; i < _weightsList.Count; i++)
        {
            for (int j = 0; j < _weightsList[i].RowCount; j++)
            {
                for (int k = 0; k < _weightsList[i].ColumnCount; k++)
                {
                    var weightsFromRandomParent = parents[UnityEngine.Random.Range(0, parents.Length - 1)].WeightsList;
                    _weightsList[i][j, k] = weightsFromRandomParent[i][j, k];
                }
            }
        }

        for (int i = 0; i < _biasesList.Count; i++)
        {
            for (int j = 0; j < _biasesList[i].ColumnCount; j++)
            {
                var biasesFromRandomParent = parents[UnityEngine.Random.Range(0, parents.Length - 1)].BiasesList;
                _biasesList[i][0, j] = biasesFromRandomParent[i][0, j];
            }
        }
    }

    private void FIllBiasesWithRandomValues()
    {
        int totalLayersCount = _hiddenLayers + 1; //Without input layer

        for (int i = 0; i < totalLayersCount; i++)
        {
            if (i == totalLayersCount - 1)
            {
                var outputBiases = GetMatrixWithRandomValues(1, _outputsCount);
                _biasesList.Add(outputBiases);
            }
            else
            {
                var hiddenBiases = GetMatrixWithRandomValues(1, _neuronsPerHiddenLayer);
                _biasesList.Add(hiddenBiases);
            }
        }
    }

    private void FillWeightsWithRandomValues()
    {
        for (int i = 0; i <= _hiddenLayers; i++)
        {
            if (i == 0)
            {
                var inputToHiddenWeights = GetMatrixWithRandomValues(_inputsCount, _neuronsPerHiddenLayer);
                _weightsList.Add(inputToHiddenWeights);
            }
            else if (i == _hiddenLayers)
            {
                var hiddenToOutputWeights = GetMatrixWithRandomValues(_neuronsPerHiddenLayer, _outputsCount);
                _weightsList.Add(hiddenToOutputWeights);
            }
            else
            {
                var hiddenToHiddenWeights = GetMatrixWithRandomValues(_neuronsPerHiddenLayer, _neuronsPerHiddenLayer);
                _weightsList.Add(hiddenToHiddenWeights);
            }
        }
    }

    private void FillRandomNeuronValues()
    {
        int totalCountOfLayers = _hiddenLayers + 2; //Hidden layers plus input and output layer

        for (int i = 0; i < totalCountOfLayers; i++)
        {
            if (i == 0)
            {
                var inputs = GetMatrixWithRandomValues(1, _inputsCount);
                _neuronsValuesList.Add(inputs);
            }
            else if (i == totalCountOfLayers - 1)
            {
                var outputs = GetMatrixWithRandomValues(1, _outputsCount);
                _neuronsValuesList.Add(outputs);
            }
            else
            {
                var hiddenNeurons = GetMatrixWithRandomValues(1, _neuronsPerHiddenLayer);
                _neuronsValuesList.Add(hiddenNeurons);
            }
        }
    }

    private Matrix<float> GetMatrixWithRandomValues(int rows, int columns)
    {
        return Matrix<float>.Build.Dense(rows, columns, (i, j) => UnityEngine.Random.Range(-1f, 1f));
    }

    public (float, float) Process(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            _neuronsValuesList[0][0, i] = inputs[i];
        }

        for (int i = 1; i < _neuronsValuesList.Count; i++)
        {
            _neuronsValuesList[i] = _neuronsValuesList[i - 1].Multiply(_weightsList[i - 1]) + _biasesList[i - 1];
            _neuronsValuesList[i] = _neuronsValuesList[i].PointwiseTanh();
        }

        var outputLayer = _neuronsValuesList.Last();

        return (Sigmoid(outputLayer[0, 0]), (float)Math.Tanh(outputLayer[0, 1]));
    }

    private float Sigmoid(double value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value));
    }
}
