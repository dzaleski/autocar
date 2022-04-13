using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] private BoardGroup boardGroup;

    public int CurrentGroup => currentGroupIndex + 1;
    public int CurrentPopulation => currentPopulation + 1;
    public NeuralNetwork BestNetwork { get; private set; }
    public static TrainingManager Instance { get; private set; }

    private NeuralNetwork[][] neuralNetworksGroups;
    private int currentGroupIndex;
    private int currentPopulation;
    private int groupSize;
    private int groupsCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        groupSize = Initializator.Instance.GroupSize;
        groupsCount = Initializator.Instance.GroupsCount;

        neuralNetworksGroups = new NeuralNetwork[groupsCount][];
    }

    public void StartTraining()
    {
        var initNeuralNetworks = GeneticManager.GetInitialNetworks();

        FillNeuralNetworkGroupsFrom(initNeuralNetworks);
        InstantiateNextCarsGroup();

        StatisticsManager.Instance.UpdateCurrentPopulationText(currentPopulation + 1);
    }

    private void FillNeuralNetworkGroupsFrom(NeuralNetwork[] neuralNetworks)
    {
        for (int i = 0; i < neuralNetworksGroups.GetLength(0); i++)
        {
            neuralNetworksGroups[i] = neuralNetworks.Skip(i * groupSize).Take(groupSize).ToArray();
        }
    }

    private void Update()
    {
        if (!DidCurrentGroupDie())
        {
            Debug.Log($"{currentGroupIndex} | {groupsCount}");
            var networks = neuralNetworksGroups.SelectMany(x => x).ToArray();
            Debug.Log(networks.Count());
            return;
        }

        Debug.Log($"{currentGroupIndex} | {groupsCount}");

        var isAnyBoardHidden = boardGroup.IsAnyBoardHidden();

        DestroyCurrentGroupCars();
        boardGroup.ResetBoards();

        if (currentGroupIndex == neuralNetworksGroups.GetLength(0))
        {
            CreateNewPopulation();

            currentGroupIndex = 0;
            currentPopulation++;

            StatisticsManager.Instance.UpdateCurrentPopulationText(currentPopulation + 1);
            StatisticsManager.Instance.UpdateBestScoreText(BestNetwork.Fitness);
        }


        if (GameManager.Instance.HideBoards || isAnyBoardHidden)
        {
            LeanTween.delayedCall(2.1f, () => InstantiateNextCarsGroup());
            return;
        }
        InstantiateNextCarsGroup();
    }

    private bool DidCurrentGroupDie()
    {
        return boardGroup.AreBoardsDisabled();
    }

    private void CreateNewPopulation()
    {
        var networks = neuralNetworksGroups.SelectMany(x => x).ToArray();

        BestNetwork = networks.OrderByDescending(x => x.Fitness).First();

        var reproducedNetworks = GeneticManager.Reproduce(networks);

        GeneticManager.Mutate(reproducedNetworks);

        FillNeuralNetworkGroupsFrom(reproducedNetworks);
    }

    private void InstantiateNextCarsGroup()
    {
        var currentGroupNetworks = neuralNetworksGroups[currentGroupIndex];

        for (int i = 0; i < currentGroupNetworks.Length; i++)
        {
            boardGroup.Items[i].SpawnCar(currentGroupNetworks[i]);
        }

        StatisticsManager.Instance.UpdateCurrentGroupText(currentGroupIndex + 1, groupsCount);
        currentGroupIndex++;
    }

    private void DestroyCurrentGroupCars()
    {
        boardGroup.DestroyCars();
    }

    public List<NeuralNetwork> GetCurrentNetworks()
    {
        return neuralNetworksGroups.SelectMany(x => x).ToList();
    }
}
