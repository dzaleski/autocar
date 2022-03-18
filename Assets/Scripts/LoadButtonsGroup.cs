using Assets.Scripts.Persistance.Repositories;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadButtonsGroup : Group<LoadButton>
{
    [SerializeField] private LoadButton loadNetworkButton;
    [SerializeField] private Networks networks;

    private void Awake()
    {
        LoadNetworks();
    }

    private void LoadNetworks()
    {
        var loadedNetworks = networks.GetAll();

        foreach (var network in loadedNetworks)
        {
            var button = Instantiate(loadNetworkButton, transform);
            button.SetCreatedDate(network.CreatedDate);
            button.SetNetworkId(network.Id);
            button.SetFitnessText(network.Fitness);
        }
    }

    public override void OnBoardPointerClick(LoadButton item)
    {
        var loadedNetwork = networks.GetById(item.NetworkId);
        SaveData.LoadedNetwork = new NeuralNetwork(loadedNetwork);
        SceneManager.LoadScene(Scenes.Testing);
    }

    public override void OnBoardPointerEnter(LoadButton item)
    {
    }

    public override void OnBoardPointerExit(LoadButton item)
    {
    }
}
