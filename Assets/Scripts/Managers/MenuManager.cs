using Assets.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartTrainingClicked()
    {
        SceneManager.LoadScene((int)Scenes.TrainParking);
    }

    public void Test1Clicked()
    {
        SceneManager.LoadScene((int)Scenes.TestParking);
    }

    public void Test2Clicked()
    {
        SceneManager.LoadScene((int)Scenes.TestParking2);
    }

    public void Test3Clicked()
    {
        SceneManager.LoadScene((int)Scenes.TestParking3);
    }
}
