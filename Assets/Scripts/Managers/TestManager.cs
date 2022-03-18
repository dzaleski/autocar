using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] private Board testingBoard;

    private void Awake()
    {
        testingBoard.InstantiateCar(SaveData.LoadedNetwork);
    }
}
