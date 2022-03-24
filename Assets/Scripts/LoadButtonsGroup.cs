using UnityEngine;

public class LoadButtonsGroup : Group<LoadButton>
{
    [SerializeField] private LoadButton loadNetworkButton;

    private void Awake()
    {
        LoadNetworks();
    }

    private void LoadNetworks()
    {
        if (SaveManager.BestNetworks == null) return;

        foreach (var network in SaveManager.BestNetworks)
        {
            var button = Instantiate(loadNetworkButton, transform);
            button.SetCreatedDate(network.CreatedDate);
            button.SetNN(new NeuralNetwork(network));
            button.SetFitnessText(network.Fitness);
        }
    }

    public override void OnBoardPointerClick(LoadButton item)
    {
        MenuManager.Instance.LoadTestSceneWithNetwork(item.Network);
    }

    public override void OnBoardPointerEnter(LoadButton item)
    {
    }

    public override void OnBoardPointerExit(LoadButton item)
    {
    }
}
