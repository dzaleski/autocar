using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Network = Assets.Scripts.Persistance.Models.Network;

public class SaveManager : MonoBehaviour
{
    public static List<Network> BestNetworks;

    public static NeuralNetwork ChoosenNetwork;

    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);

        Load();
    }

    public void Load()
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        var formatter = GetBinaryFormatter();
        var file = File.Open(filePath, FileMode.Open);

        try
        {
            var save = formatter.Deserialize(file);
            file.Close();
            BestNetworks = save as List<Network>;
        }
        catch (Exception)
        {
            Debug.LogErrorFormat("Failed to load file at {0}", filePath);
            file.Close();
        }
    }

    public void Save(Network network)
    {
        if(BestNetworks == null)
        {
            BestNetworks = new List<Network>();
        }

        BestNetworks.Add(network);
        var formatter = GetBinaryFormatter();
        var file = File.Create(filePath);
        formatter.Serialize(file, BestNetworks);
        file.Close();
    }

    private BinaryFormatter GetBinaryFormatter()
    {
        var formatter = new BinaryFormatter();
        return formatter;
    }

    private string filePath => $@"{Application.persistentDataPath}\networks.save";
}
