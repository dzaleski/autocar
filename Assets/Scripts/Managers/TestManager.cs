using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] private Board testingBoard;
    [SerializeField] private float timeScale;

    private void Awake()
    {
        testingBoard.SpawnCar(SaveManager.ChoosenNetwork);
    }
}
