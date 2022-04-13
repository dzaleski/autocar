using System;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Persistance.Models;
using MathNet.Numerics.LinearAlgebra;
using Random = UnityEngine.Random;

[Serializable]
public class NeuralNetwork
{
    public float Fitness { get; set; }
    public int HiddenLayers => _hiddenLayers;
    public int NeuronsPerHiddenLayer => _neuronsPerHiddenLayer;
    public List<Matrix<float>> MatrixesOfWeights => new List<Matrix<float>>(_matrixesOfWeights);
    public List<Matrix<float>> ValuesOfBiases => new List<Matrix<float>>(_valuesOfBiases);

    private static int _inputs;
    private static int _hiddenLayers;
    private static int _neuronsPerHiddenLayer;
    private static int _outputs;

    private List<Matrix<float>> _matrixesOfWeights;
    private List<Matrix<float>> _valuesOfNeurons;
    private List<Matrix<float>> _valuesOfBiases;

    public NeuralNetwork()
    {
        _valuesOfNeurons = GetMatrixesOfNeuronValues().ToList();
        _valuesOfBiases = GetMatrixesOfBiases().ToList();
        _matrixesOfWeights = GetMatrixesOfWeights().ToList();
    }

    public NeuralNetwork(params NeuralNetwork[] parents) : this()
    {
        ApplyWeightsFromParents(parents);
        ApplyBiasesFromParents(parents);
    }

    private void ApplyWeightsFromParents(NeuralNetwork[] parents)
    {
        for (int i = 0; i < _matrixesOfWeights.Count; i++)
        {
            for (int j = 0; j < _matrixesOfWeights[i].RowCount; j++)
            {
                for (int k = 0; k < _matrixesOfWeights[i].ColumnCount; k++)
                {
                    var randomParent = parents[Random.Range(0, parents.Length)];
                    var weightsFromRandomParent = randomParent.MatrixesOfWeights[i];
                    _matrixesOfWeights[i][j, k] = weightsFromRandomParent[j, k];
                }
            }
        }
    }

    private void ApplyBiasesFromParents(NeuralNetwork[] parents)
    {
        for (int i = 0; i < _valuesOfBiases.Count; i++)
        {
            for (int j = 0; j < _valuesOfBiases[i].RowCount; j++)
            {
                for (int k = 0; k < _valuesOfBiases[i].ColumnCount; k++)
                {
                    var randomParent = parents[Random.Range(0, parents.Length)];
                    var biasesFromRandomParent = randomParent.ValuesOfBiases[i];
                    _valuesOfBiases[i][j, k] = biasesFromRandomParent[j, k];
                }
            }
        }
    }

    public NeuralNetwork(NeuralNetwork networkToCopy) : this()
    {
        CopyWeightsFromTo(networkToCopy, this);
        CopyBiasesFromTo(networkToCopy, this);
    }

    public void CopyWeightsFromTo(NeuralNetwork from, NeuralNetwork to)
    {
        _matrixesOfWeights.Clear();
        foreach (var weightMatrix in from._matrixesOfWeights)
        {
            to._matrixesOfWeights.Add(weightMatrix);
        }
    }

    public void CopyBiasesFromTo(NeuralNetwork from, NeuralNetwork to)
    {
        _valuesOfBiases.Clear();
        foreach (var weightMatrix in from._valuesOfBiases)
        {
            to._valuesOfBiases.Add(weightMatrix);
        }
    }

    public NeuralNetwork(Network network) : this()
    {
        _matrixesOfWeights.Clear();
        _valuesOfBiases.Clear();

        //Copy weights from saved network
        foreach (var weightMatrix in network.MatrixesOfWeights)
        {
            _matrixesOfWeights.Add(weightMatrix);
        }

        //Copy bias from saved network
        foreach (var bias in network.ValuesOfBiases)
        {
            _valuesOfBiases.Add(bias);
        }

        //Debug.Log("First column" + string.Join(", ", _matrixesOfWeights[0].Column(0)));
        //Debug.Log("Second column" + string.Join(", ", _matrixesOfWeights[0].Column(1)));
        //Debug.Log("Third column" + string.Join(", ", _matrixesOfWeights[0].Column(2)));
    }

    private IEnumerable<Matrix<float>> GetMatrixesOfNeuronValues()
    {
        // If _inputs = 8, matrix will have 8 x 1 size, so its just a vector
        yield return Matrix<float>.Build.Dense(_inputs, 1);

        for (int i = 0; i < _hiddenLayers; i++)
        {
            yield return Matrix<float>.Build.Dense(_neuronsPerHiddenLayer, 1);
        }

        yield return Matrix<float>.Build.Dense(_outputs, 1);
    }

    private IEnumerable<Matrix<float>> GetMatrixesOfBiases()
    {
        // If _inputs = 8, matrix will have 8 x 1 size, so its just a vector
        yield return GetMatrixFilledWithRandomValues(_inputs, 1);

        for (int i = 0; i < _hiddenLayers; i++)
        {
            yield return GetMatrixFilledWithRandomValues(_neuronsPerHiddenLayer, 1);
        }

        yield return GetMatrixFilledWithRandomValues(_outputs, 1);
    }

    public Matrix<float> GetMatrixFilledWithRandomValues(int rows, int columns)
    {
        return Matrix<float>.Build.Dense(rows, columns, (_, _) => Random.Range(-1f, 1f));
    }

    private IEnumerable<Matrix<float>> GetMatrixesOfWeights()
    {
        yield return GetMatrixFilledWithRandomValues(_neuronsPerHiddenLayer, _inputs);

        for (int i = 0; i < _hiddenLayers - 1; i++)
        {
            yield return GetMatrixFilledWithRandomValues(_neuronsPerHiddenLayer, _neuronsPerHiddenLayer);
        }

        yield return GetMatrixFilledWithRandomValues(_outputs, _neuronsPerHiddenLayer);
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
        for (int i = 0; i < _inputs; i++)
        {
            //Fill first vector with input values
            _valuesOfNeurons[0][i, 0] = inputs[i];
        }

        for (int i = 1; i < _valuesOfNeurons.Count; i++)
        {
            _valuesOfNeurons[i] = _matrixesOfWeights[i - 1] * _valuesOfNeurons[i - 1] + _valuesOfBiases[i];
            _valuesOfNeurons[i] = _valuesOfNeurons[i].PointwiseTanh();
        }

        //Matrix of size _output x 1, so we are getting first column
        var outputLayer = _valuesOfNeurons.Last().Column(0);

        return outputLayer.ToArray();
    }

    public void Mutate(float mutationProb)
    {
        for (int i = 0; i < _matrixesOfWeights.Count; i++)
        {
            for (int rowIndex = 0; rowIndex < _matrixesOfWeights[i].RowCount; rowIndex++)
            {
                if (!ShouldMutate(mutationProb))
                {
                    continue;
                }

                var randomColumn = Random.Range(0, _matrixesOfWeights[i].ColumnCount);
                _matrixesOfWeights[i][rowIndex, randomColumn] = Random.Range(-1f, 1f);
            }
        }

        for (int i = 0; i < _valuesOfBiases.Count; i++)
        {
            for (int rowIndex = 0; rowIndex < _valuesOfBiases[i].RowCount; rowIndex++)
            {
                if (!ShouldMutate(mutationProb))
                {
                    continue;
                }

                _valuesOfBiases[i][rowIndex, 0] = Random.Range(-1f, 1f);
            }
        }
    }

    private bool ShouldMutate(float mutationProb)
    {
        float randomValueBetween01 = Random.Range(0f, 1f);

        if (randomValueBetween01 < mutationProb)
        {
            return true;
        }

        return false;
    }
}