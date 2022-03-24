using System;
using System.Collections.Generic;

namespace Assets.Scripts.Persistance.Models
{
    [Serializable]
    public class Network : Base
    {
        public Network(List<float[,]> weightsBetweenTheLayers, float fitness) : base()
        {
            WeightsBetweenTheLayers = weightsBetweenTheLayers;
            Fitness = fitness;
        }

        public List<float[,]> WeightsBetweenTheLayers;
        public float Fitness;
    }
}
