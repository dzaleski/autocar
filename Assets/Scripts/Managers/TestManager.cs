using Assets.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AutoCar carPrefab;
    [SerializeField] private Transform startPoint;

    private AutoCar currentCar;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            currentCar = InstantiatneCarFromBestNN();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            RestartCar();
        }
    }

    private AutoCar InstantiatneCarFromBestNN()
    {
        var car = Instantiate(carPrefab);
        car.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation); 
        return car;
    }

    private void RestartCar()
    {
        Destroy(currentCar);
        currentCar = InstantiatneCarFromBestNN();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene((int)Scenes.MainMenu);
    }
}
