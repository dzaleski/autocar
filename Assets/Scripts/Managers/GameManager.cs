using UnityEngine;
using UnityEngine.SceneManagement;
using Network = Assets.Scripts.Persistance.Models.Network;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool HideBoards => hideBoards;

    [SerializeField] private int timeScale = 4;
    [SerializeField] private BoardGroup boardGroup;

    private bool hideBoards = true;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        boardGroup.InstantiateMaps();
        CameraController.Instance.SetCameraToStartPosition();
        TrainingManager.Instance.StartTraining();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CameraController.Instance.SetCameraToStartPosition();
        }
    }
    public void SaveBestNetwork()
    {
        var bestNetwork = TrainingManager.Instance.BestNetwork;

        if(bestNetwork == null)
        {
            return;
        }

        var networkToSave = new Network(bestNetwork.weightsBetweenTheLayers, bestNetwork.Fitness, bestNetwork.HiddenLayers, bestNetwork.NeuronsPerHiddenLayer);
        SaveManager.Instance.Save(networkToSave);
    }

    public void SaveObserverNetwork()
    {
        var bestNetwork = TrainingManager.Instance.BestNetwork;

        if (bestNetwork == null)
        {
            return;
        }

        var networkToSave = new Network(bestNetwork.weightsBetweenTheLayers, bestNetwork.Fitness, bestNetwork.HiddenLayers, bestNetwork.NeuronsPerHiddenLayer);
        SaveManager.Instance.SaveToObserve(networkToSave);
    }

    public void SetHideBoards(bool isVisible)
    {
        hideBoards = isVisible;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(Scenes.Menu);
    }
}
