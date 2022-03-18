using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    public int CurrentGroup => currentGroupIndex + 1;
    public int CurrentPopulation => currentPopulation + 1;
    public NeuralNetwork BestNetwork { get; private set; }

    private static TrainingManager instance;
    public static TrainingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TrainingManager>();
            }

            return instance;
        }
    }

    [Header("Other")]
    [SerializeField] private int timeScale = 4;
    [SerializeField] private BoardGroup boardGroup;
    [SerializeField] private Board boardPrefab;

    private NeuralNetwork[][] neuralNetworksGroups;
    private int currentGroupIndex;
    private int currentPopulation;
    private int groupSize;
    private int groupsCount;

    private void Start()
    {
        groupSize = Initializator.Instance.GroupSize;
        groupsCount = Initializator.Instance.GroupSize;

        float columns = Mathf.Ceil(Mathf.Sqrt(groupSize));
        float rows = Mathf.Ceil(groupSize / columns);

        InstantiateMaps(columns);
        AdjustCameraPosition(rows, columns);

        neuralNetworksGroups = new NeuralNetwork[groupsCount][];

        var initNeuralNetworks = GeneticManager.GetInitialNetworks();

        FillNeuralNetworkGroupsFrom(initNeuralNetworks);
        InstantiateNextCarsGroup();
    }

    private void AdjustCameraPosition(float rows, float columns)
    {
        var (width, center) = GetGridWidthAndCenter(rows, columns);

        const float margin = 1.1f;
        float maxExtent = width / 4;
        float minDistance = (maxExtent * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f);
        Camera.main.transform.position = center - Camera.main.transform.forward * minDistance;
        Camera.main.nearClipPlane = minDistance - maxExtent;
    }

    private (float width, Vector3 gridCenter) GetGridWidthAndCenter(float rows, float columns)
    {
        var singleBoardSize = boardGroup.Items.First().GetBoardSize();

        var verticalSize = singleBoardSize.vertical * (rows - 2);
        var horizontalSize = singleBoardSize.horizontal * columns;

        var horizontalShift = Vector3.right * (horizontalSize / 2);
        var verticalShift = Vector3.forward * (verticalSize / 2);

        var gridCenter = boardGroup.transform.position + horizontalShift - verticalShift;

        return (horizontalSize, gridCenter);
    }

    private void InstantiateMaps(float columns)
    {
        int currentRow = 0;
        int currentColumn = 0;

        for (int i = 1; i <= groupSize; i++)
        {
            var board = Instantiate(boardPrefab, boardGroup.transform);
            board.PlaceBoardOnGrid(currentRow, currentColumn);

            currentColumn++;

            if (i % columns == 0)
            {
                currentRow++;
                currentColumn = 0;
            }
        }
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
        Time.timeScale = timeScale;

        if (!DidCurrentGroupDie()) return;

        DestroyCurrentGroupCars();

        if (currentGroupIndex >= neuralNetworksGroups.GetLength(0))
        {
            CreateNewPopulation();
            currentGroupIndex = 0;
            currentPopulation++;
        }

        InstantiateNextCarsGroup();
    }

    private bool DidCurrentGroupDie()
    {
        int diedCarsCount = boardGroup.Items.Count(x => x.IsCarDisabled);
        return diedCarsCount == groupSize;
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
            boardGroup.Items[i].InstantiateCar(currentGroupNetworks[i]);
        }

        currentGroupIndex++;
    }

    private void DestroyCurrentGroupCars()
    {
        foreach (var board in boardGroup.Items)
        {
            board.DestroyCar();
        }
    }

    public List<NeuralNetwork> GetCurrentNetworks()
    {
        return neuralNetworksGroups.SelectMany(x => x).ToList();
    }

    public void SetTimeScale(int scale)
    {
        timeScale = scale;
    }
}
