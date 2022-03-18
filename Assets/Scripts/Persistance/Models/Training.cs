using System;
using System.Collections.Generic;

namespace Assets.Scripts.Persistance.Models
{
    [Serializable]
    public class Training : Base
    {
        public Training(List<Network> networks) : base()
        {
            Networks = networks;
        }

        public List<Network> Networks { get; set; }
    }
}
