using Assets.Scripts.Persistance.Models;
using Assets.Scripts.Persistance.Repositories;
using System.Collections.Generic;
using UnityEngine;
using Network = Assets.Scripts.Persistance.Models.Network;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Trainings trainings;
    [SerializeField] private Networks networks;

    public void SaveTraining()
    {
        var currentNetworks = TrainingManager.Instance.GetCurrentNetworks();

        var networksToSave = new List<Network>();

        foreach (var network in currentNetworks)
        {
            networksToSave.Add(new Network(network.weightsBetweenTheLayers));
        }

        trainings.Add(new Training(networksToSave));
        trainings.Save();
    }

    public void SaveBestNetwork()
    {
        var bestNetwork = TrainingManager.Instance.BestNetwork;
        networks.Add(new Network(bestNetwork.weightsBetweenTheLayers));
        networks.Save();
    }
}
