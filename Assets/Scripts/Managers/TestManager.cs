using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    [SerializeField] private BoardGroup boardGroup;
    [SerializeField] private Board testingBoard;
    [SerializeField] private float timeScale;

    private NeuralNetwork loadedNeuralNetwork;
    private float startTimeScale;

    private void Awake()
    {
        NeuralNetwork.Initialise(8, SaveManager.ChoosenNetwork.HiddenLayers, SaveManager.ChoosenNetwork.NeuronsPerHiddenLayer, 3);
        loadedNeuralNetwork = new NeuralNetwork(SaveManager.ChoosenNetwork);
        StartTest();
        startTimeScale = Time.timeScale;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(Scenes.Menu);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    private void StartTest()
    {
        testingBoard.SpawnCar(loadedNeuralNetwork, true);
    }


    public void Restart()
    {
        testingBoard.DestroyCar();
        StartTest();
        SetTimeScale(startTimeScale);
    }
}
