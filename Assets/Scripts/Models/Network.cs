using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Persistance.Models
{
    [Serializable]
    public class Network : Base
    {
        public List<Matrix<float>> MatrixesOfWeights { get; set; }
        public List<Matrix<float>> ValuesOfBiases { get; set; }
        public float Fitness { get; set; }
        public int HiddenLayers { get; set; }
        public int NeuronsPerHiddenLayer { get; set; }

        public static Network FromNeuralNetwork(NeuralNetwork neuralNetwork)
        {
            return new Network
            {
                MatrixesOfWeights = neuralNetwork.MatrixesOfWeights,
                ValuesOfBiases = neuralNetwork.ValuesOfBiases,
                Fitness = neuralNetwork.Fitness,
                HiddenLayers = neuralNetwork.HiddenLayers,
                NeuronsPerHiddenLayer = neuralNetwork.NeuronsPerHiddenLayer
            };
        }
    }
}
