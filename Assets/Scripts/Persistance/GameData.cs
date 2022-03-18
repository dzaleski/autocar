using Assets.Scripts.Persistance.Models;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Persistance
{
    [Serializable]
    public class GameData
    {
        public List<Network> BestNetworks { get; set; } = new List<Network>();
        public List<Training> Trainings { get; set; } = new List<Training>();
    }
}
