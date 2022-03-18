using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public bool IsCarDisabled => car.IsDisabled;

    [Header("Transforms")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform carsHolder;
    [SerializeField] private Transform parkingSpot;

    [Header("Prefabs")]
    [SerializeField] private AutoCar carPrefab;
    [SerializeField] private NavMeshData m_NavMeshData;

    private BoardGroup boardGroup;
    private AutoCar car;
    private BoxCollider boxCollider;
    private NavMeshDataInstance navMeshDataInstance;

    private void Awake()
    {
        boardGroup = transform.parent.GetComponent<BoardGroup>();
        boardGroup.Subscribe(this);
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnDisable()
    {
        NavMesh.RemoveNavMeshData(navMeshDataInstance);
    }

    private void Start()
    {
        navMeshDataInstance = NavMesh.AddNavMeshData(m_NavMeshData, transform.position, Quaternion.identity);
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
        var instantiatedCar = Instantiate(carPrefab, carsHolder);

        instantiatedCar.SetNeuralNetwork(neuralNetwork);
        instantiatedCar.SetParkingSpot(parkingSpot);
        instantiatedCar.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);

        car = instantiatedCar;
    }

    public void DisableCar()
    {
        car.Disable();
    }

    public void DestroyCar()
    {
        Destroy(car);
    }

    public void PlaceBoardOnGrid(int row, int column)
    {
        var boardSize = GetBoardSize();
        var rightShift = transform.right * boardSize.horizontal * column;
        var leftShift = -transform.forward * boardSize.vertical * row;

        transform.position += rightShift + leftShift;
    }

    public (float horizontal, float vertical) GetBoardSize()
    {
        float spaceBetweenMaps = 5f;

        float horizontal = boxCollider.bounds.size.x + spaceBetweenMaps;
        float vertical = boxCollider.bounds.size.z + spaceBetweenMaps;
        return (horizontal, vertical);
    }
}
