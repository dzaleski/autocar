using UnityEngine;
using UnityEngine.SceneManagement;
using Network = Assets.Scripts.Persistance.Models.Network;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

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
    }

    public void LoadTrainScene()
    {
        Initializator.Instance.InitializeAll();
        SceneManager.LoadScene(Scenes.Training);
    }

    public void LoadTestSceneWithNetwork(Network network)
    {
        SaveManager.ChoosenNetwork = network;
        LoadTestScene();
    }

    public void LoadTestSceneWithPretrainedNetwork()
    {
        SaveManager.ChoosenNetwork = SaveManager.Pretrained;
        LoadTestScene();
    }

    private static void LoadTestScene()
    {
        SceneManager.LoadScene(Scenes.Testing);
    }
}
