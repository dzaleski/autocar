using UnityEngine;
using Network = Assets.Scripts.Persistance.Models.Network;

public class GameManager : MonoBehaviour
{
    public bool HideBoards = true;
    [SerializeField] private int timeScale = 4;
    [SerializeField] private BoardGroup boardGroup;

    public static GameManager Instance { get; private set; }

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
    }

    public void SaveBestNetwork()
    {
        var bestNetwork = TrainingManager.Instance.BestNetwork;
        var networkToSave = new Network(bestNetwork.weightsBetweenTheLayers, bestNetwork.Fitness);
        SaveManager.Instance.Save(networkToSave);
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

        Time.timeScale = timeScale;
    }
}
