using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralNetwork
{
    private int _inputsCount;
    private int _neuronsPerHiddenLayer;
    private int _hiddenLayers;
    private int _outputsCount;
    public List<Matrix<double>> WeightsList { get; private set; }
    public List<Matrix<double>> BiasesList { get; private set; }

    private List<Matrix<double>> _neuronsValuesList;

    public float fitness;

    public NeuralNetwork(int inputs, int hiddenLayers, int neuronsPerHiddenLayer, List<NeuralNetwork> parents = null)
    {
        _inputsCount = inputs;
        _neuronsPerHiddenLayer = neuronsPerHiddenLayer;
        _hiddenLayers = hiddenLayers;
        _outputsCount = 2;
        _neuronsValuesList = new List<Matrix<double>>();
        WeightsList = new List<Matrix<double>>();
        BiasesList = new List<Matrix<double>>();
        fitness = 0;

        InitBiasesList();
        InitWeightsList();
        InitNeuronValuesList();

        if (parents == null)
        {
            FillListOfMatrixesWithRandomValues(WeightsList);
            FillListOfMatrixesWithRandomValues(BiasesList);
        }
        else
        {

            Debug.Log($"My parent fitenss: {parents.First().fitness}");
            GetEachWeightFromRandomParent(parents);
            GetEachBiasesFromRandomParent(parents);
        }
        
    }

    private void GetEachWeightFromRandomParent(List<NeuralNetwork> parents)
    {
        for (int i = 0; i < WeightsList.Count; i++)
        {
            for (int j = 0; j < WeightsList[i].RowCount; j++)
            {
                for (int k = 0; k < WeightsList[i].ColumnCount; k++)
                {
                    var weightsFromRandomParent = parents[UnityEngine.Random.Range(0, parents.Count)].WeightsList;
                    WeightsList[i][j, k] = weightsFromRandomParent[i][j, k];
                }
            }
        }
    }

    private void GetEachBiasesFromRandomParent(List<NeuralNetwork> parents)
    {
        for (int i = 0; i < BiasesList.Count; i++)
        {
            for (int k = 0; k < BiasesList[i].RowCount; k++)
            {
                var weightsFromRandomParent = parents[UnityEngine.Random.Range(0, parents.Count)].BiasesList;
                BiasesList[i][0, k] = weightsFromRandomParent[i][0, k];
            }
        }
    }

    private void InitBiasesList()
    {
        BiasesList.Clear();

        int totalLayersCount = _hiddenLayers + 1; //Without input layer

        for (int i = 0; i < totalLayersCount; i++)
        {
            if (i == totalLayersCount - 1)
            {
                var outputBiases = GetEmptyMatrix(1, _outputsCount);
                BiasesList.Add(outputBiases);
            }
            else
            {
                var hiddenBiases = GetEmptyMatrix(1, _neuronsPerHiddenLayer);
                BiasesList.Add(hiddenBiases);
            }
        }
    }

    private void InitWeightsList()
    {
        WeightsList.Clear();

        for (int i = 0; i <= _hiddenLayers; i++)
        {
            if (i == 0)
            {
                var inputToHiddenWeights = GetEmptyMatrix(_inputsCount, _neuronsPerHiddenLayer);
                WeightsList.Add(inputToHiddenWeights);
            }
            else if (i == _hiddenLayers)
            {
                var hiddenToOutputWeights = GetEmptyMatrix(_neuronsPerHiddenLayer, _outputsCount);
                WeightsList.Add(hiddenToOutputWeights);
            }
            else
            {
                var hiddenToHiddenWeights = GetEmptyMatrix(_neuronsPerHiddenLayer, _neuronsPerHiddenLayer);
                WeightsList.Add(hiddenToHiddenWeights);
            }
        }
    }

    private void InitNeuronValuesList()
    {
        _neuronsValuesList.Clear();

        int totalLayers = _hiddenLayers + 2;

        for (int i = 0; i < totalLayers; i++)
        {
            if (i == 0)
            {
                var inputNeurons = GetEmptyMatrix(1, _inputsCount);
                _neuronsValuesList.Add(inputNeurons);
            }
            else if (i == _hiddenLayers)
            {
                var outputNeurons = GetEmptyMatrix(1, _neuronsPerHiddenLayer);
                _neuronsValuesList.Add(outputNeurons);
            }
            else
            {
                var hiddenNeurons = GetEmptyMatrix(1, _outputsCount);
                _neuronsValuesList.Add(hiddenNeurons);
            }
        }
    }

    private void FillListOfMatrixesWithRandomValues(List<Matrix<double>> listOfMatrixes)
    {
        foreach (var matrix in listOfMatrixes)
        {
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    matrix[i, j] = UnityEngine.Random.Range(-1f, 1f);
                }
            }
        }
    }

    private Matrix<double> GetEmptyMatrix(int rows, int columns)
    {
        return Matrix<double>.Build.Dense(rows, columns);
    }

    public (float, float) Process(List<float> inputs)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            _neuronsValuesList[0][0, i] = inputs[i];
        }

        for (int i = 1; i < _neuronsValuesList.Count; i++)
        {
            _neuronsValuesList[i] = _neuronsValuesList[i - 1].Multiply(WeightsList[i - 1]) + BiasesList[i -1];
        }

        var outputLayer = _neuronsValuesList.Last();

        return ((float)Math.Tanh(outputLayer[0, 0]), (float)Math.Tanh(outputLayer[0, 1]));
    }

    private float Sigmoid(double value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value));
    }

    public void Mutate(float mutationProb)
    {
        float randomValueBetween01 = UnityEngine.Random.Range(0f, 1f);

        foreach (var weightMatrix in WeightsList)
        {
            for (int i = 0; i < weightMatrix.RowCount; i++)
            {
                if (randomValueBetween01 > mutationProb)
                {
                    continue;
                }

                int randomColumnIndex = UnityEngine.Random.Range(0, weightMatrix.ColumnCount);

                weightMatrix[i, randomColumnIndex] = UnityEngine.Random.Range(-1f, 1f);
            }
        }
    }
}
