using Assets.Scripts.Persistance.Models;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    private static SaveData _current;
    public static SaveData Current
    {
        get 
        { 
            if(_current == null)
            {
                _current = new SaveData();
            }

            return _current;
        }
    }

    public List<Network> BestNetworks;
    public List<Training> Trainnigs;
}
