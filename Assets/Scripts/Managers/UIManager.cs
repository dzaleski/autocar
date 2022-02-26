using Assets.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currGenerationText;

    private int currentGeneration = 1;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
    }

    public void UpdateCurrentGenerationText()
    {
        //currentGeneration++;
        //currGenerationText.SetText($"Current generation: <color=green>{currentGeneration}</color>");
    }

    public void LoadParkingScene()
    {
        SceneManager.LoadScene((int)Scenes.TrainParking);
    }

    public void LoadTrackScene()
    {
        SceneManager.LoadScene((int)Scenes.Track);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene((int)Scenes.MainMenu);
    }
}
