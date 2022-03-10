using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public BoardGroup boardGroup;

    [Header("Transforms")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform carsHolder;
    [SerializeField] private Transform parkedCarsParent;
    [SerializeField] private Transform parkingSpot;

    [Header("Prefabs")]
    [SerializeField] private AutoCar carPrefab;

    private void Awake()
    {
        boardGroup.Subscribe(this);
    }

    public AutoCar CreateBrain(NeuralNetwork neuralNetwork)
    {
        var car = Instantiate(carPrefab, carsHolder);
        car.SetNeuralNetwork(neuralNetwork);
        car.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        return car;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        boardGroup.OnBoardPointerClick(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        boardGroup.OnBoardPointerEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        boardGroup.OnBoardPointerExit(this);
    }

    public AutoCar InstantiateCar(NeuralNetwork neuralNetwork)
    {
        var car = Instantiate(carPrefab, carsHolder);
        car.SetNeuralNetwork(neuralNetwork);
        car.ParkingSpot = parkingSpot;
        car.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        return car;
    }
}
