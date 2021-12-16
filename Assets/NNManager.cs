using UnityEngine;

public class NNManager : MonoBehaviour
{
    private static NNManager _instance;
    public static NNManager Instance => _instance;

    [Range(1,3)] public int countOfHiddenLayers;
    [Range(3, 5)] public int countOfNeruonPerHiddenLayer;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
