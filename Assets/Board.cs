using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public BoardGroup boardGroup;
    public bool IsCarDisabled => boardCar.Disabled;

    [Header("Transforms")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform carsHolder;
    [SerializeField] private Transform parkedCarsParent;
    [SerializeField] private Transform parkingSpot;

    [Header("Prefabs")]
    [SerializeField] private AutoCar carPrefab;

    private AutoCar boardCar;
    private ParkedCar[] parkedCars;

    private void Awake()
    {
        boardGroup.Subscribe(this);
        parkedCars = parkedCarsParent.GetComponentsInChildren<ParkedCar>();
    }

    public void RestartParkedCars()
    {
        foreach (var car in parkedCars)
        {
            car.RestartPosition();
        }
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

    public void InstantiateCar(NeuralNetwork neuralNetwork)
    {
        var car = Instantiate(carPrefab, carsHolder);
        car.SetNeuralNetwork(neuralNetwork);
        car.ParkingSpot = parkingSpot;
        car.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        boardCar = car;
    }

    public void DisableCar()
    {
        boardCar.Disable();
    }

    public void DestroyCar()
    {
        Destroy(boardCar);
    }
}
