using System;
using System.Collections.Generic;

namespace Assets.Scripts.Persistance.Models
{
    [Serializable]
    public class Network : Base
    {
        public Network(List<float[,]> weightsBetweenTheLayers) : base()
        {
            this.weightsBetweenTheLayers = weightsBetweenTheLayers;
        }

        public List<float[,]> weightsBetweenTheLayers { get; set; }
    }
}
