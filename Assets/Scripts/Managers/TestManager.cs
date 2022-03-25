using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    [SerializeField] private BoardGroup boardGroup;
    [SerializeField] private Board testingBoard;
    [SerializeField] private float timeScale;

    private void Awake()
    {
        testingBoard.SpawnCar(SaveManager.ChoosenNetwork, true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(Scenes.Menu);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
