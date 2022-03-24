using UnityEngine;

public class Initializator : MonoBehaviour
{
    [Header("Genetic Algorithm")]
    public float MutationProb = 0.055f;
    public int GroupSize = 4;
    public int GroupsCount = 4;
    public float PercentOfTheBestPass = 0.1f;
    public float PercentOfRandom = 0.1f;

    [Header("Neural Network")]
    public int Inputs = 8;
    public int HiddenLayers = 2;
    public int NeuronsPerHiddenLayer = 12;
    public int Outputs = 2;

    [Header("Sensors")]
    public int Length = 12;
    public LayerMask RayMask;
    public bool IsVisible = true;

    [HideInInspector] public int PopulationSize;
    public static Initializator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        PopulationSize = GroupsCount * GroupSize;
        GeneticManager.Initialise(PopulationSize, MutationProb, PercentOfTheBestPass, PercentOfRandom);
        NeuralNetwork.Initialise(Inputs, HiddenLayers, NeuronsPerHiddenLayer, Outputs);
        SensorsController.Initialise(Inputs, Length, RayMask, IsVisible);
    }

    public void SetGroupSize(float value)
    {
        GroupSize = (int)value;
    }

    public void SetGroupsCount(float value)
    {
        GroupsCount = (int)value;
    }

    public void SetIsVisible(bool isVisible)
    {
        IsVisible = isVisible;
    }
}
