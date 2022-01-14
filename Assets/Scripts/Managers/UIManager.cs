using Assets.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currGenerationText;

    private int currentGeneration = 1;

    public void UpdateCurrentGenerationText()
    {
        currentGeneration++;
        currGenerationText.SetText($"Current generation: <color=green>{currentGeneration}</color>");
    }

    public void LoadParkingScene()
    {
        SceneManager.LoadScene((int)Scenes.Parking);
    }

    public void LoadTrackScene()
    {
        SceneManager.LoadScene((int)Scenes.Track);
    }
}
