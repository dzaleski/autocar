using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(Scenes.Training);
    }

    public void LoadTestSceneWithNetwork(NeuralNetwork network)
    {
        SaveManager.ChoosenNetwork = network;
        LoadTestScene();
    }

    public void LoadTestSceneWithPretrainedNetwork()
    {
        SaveManager.ChoosenNetwork = new NeuralNetwork(SaveManager.Pretrained);
        LoadTestScene();
    }

    private static void LoadTestScene()
    {
        SceneManager.LoadScene(Scenes.Testing);
    }
}
