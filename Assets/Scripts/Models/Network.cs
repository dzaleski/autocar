using System;
using System.Collections.Generic;

namespace Assets.Scripts.Persistance.Models
{
    [Serializable]
    public class Network : Base
    {
        public Network(List<float[,]> weightsBetweenTheLayers, float fitness, int hiddenLayers, int neuronsPerHiddenLayer)
        {
            WeightsBetweenTheLayers = weightsBetweenTheLayers;
            Fitness = fitness;
            HiddenLayers = hiddenLayers;
            NeuronsPerHiddenLayer = neuronsPerHiddenLayer;
        }

        public List<float[,]> WeightsBetweenTheLayers { get; set; }
        public float Fitness { get; set; }
        public int HiddenLayers { get; set; }
        public int NeuronsPerHiddenLayer { get; set; }
    }
}
