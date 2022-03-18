using Assets.Scripts.Persistance.Models;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Persistance
{
    [Serializable]
    public abstract class DataContext
    {
        public GameData data = new GameData();

        public abstract void Save();

        public abstract void Load();

        public List<T> Set<T>() 
        {
            if(typeof(T) == typeof(Network))
            {
                return data.BestNetworks as List<T>;
            }

            if (typeof(T) == typeof(Training))
            {
                return data.Trainings as List<T>;
            }

            return null;
        }
    }
}
