using Assets.Scripts.Persistance.Repositories;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Repositories")]
    [SerializeField] private Trainings trainings;

    [Header("Parents")]
    [SerializeField] private Transform loadedNetworksPanel;

    public void LoadTrainScene()
    {
        SceneManager.LoadScene(Scenes.Training);
    }
}
