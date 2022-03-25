using UnityEngine;
using UnityEngine.AI;

public class Board : MonoBehaviour
{
    public bool IsDisabled => car != null && car.IsDisabled;
    public bool IsHidden => isHidden;

    [Header("Transforms")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform carsHolder;
    [SerializeField] private Transform parkingSpot;

    [Header("Prefabs")]
    [SerializeField] private AutoCar carPrefab;
    [SerializeField] private NavMeshData m_NavMeshData;

    public AnimationCurve animationCurve;

    private BoardGroup boardGroup;
    private AutoCar car;
    private BoxCollider boxCollider;
    private NavMeshDataInstance navMeshDataInstance;

    private Vector3 startPos;
    private Vector3 disablePos;

    private float hideBoardTime = 2f;
    private bool isHidden = false;

    private void Awake()
    {
        boardGroup = transform.parent.GetComponent<BoardGroup>();
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

    public void SpawnCar(NeuralNetwork neuralNetwork, bool test = false)
    {
        car = Instantiate(carPrefab, carsHolder);
        car.IsTesting = test;
        car.SetNeuralNetwork(neuralNetwork);
        car.SetParkingSpot(parkingSpot);
        car.SetStartLossValue();
        car.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        car.OnDisable = TurnOffBoard;
    }

    public static Board InstantiateBoard(Board boardPrefab, BoardGroup boardGroup)
    {
        var board = Instantiate(boardPrefab, boardGroup.transform);
        boardGroup.Subscribe(board);
        return board;
    }

    public void DestroyCar()
    {
        if (car == null) return;

        Destroy(car.gameObject);
        car = null;
    }

    public void PlaceBoardOnGrid(int row, int column)
    {
        var boardSize = GetBoardSize();
        var rightShift = transform.right * boardSize.horizontal * column;
        var leftShift = -transform.forward * boardSize.vertical * row;

        transform.position += rightShift + leftShift;
        startPos = transform.position;
        disablePos = startPos - transform.up * 78f;
    }

    public (float horizontal, float vertical) GetBoardSize()
    {
        float spaceBetweenMaps = 5f;

        float horizontal = boxCollider.bounds.size.x + spaceBetweenMaps;
        float vertical = boxCollider.bounds.size.z + spaceBetweenMaps;
        return (horizontal, vertical);
    }

    private void OnMouseDown()
    {
        boardGroup.OnBoardPointerClick(this);
    }

    private void OnMouseEnter()
    {
        boardGroup.OnBoardPointerEnter(this);
    }

    private void OnMouseExit()
    {
        boardGroup.OnBoardPointerExit(this);
    }

    public Vector3 GetBoardCenter()
    {
        return boxCollider.bounds.center;
    }

    public void TurnOffBoard()
    {
        if (!GameManager.Instance.HideBoards) return;

        isHidden = true;

        LeanTween.move(gameObject, disablePos, hideBoardTime).setEase(animationCurve);
    }

    public void TurnOnBoard()
    {
        isHidden = false;
        LeanTween.move(gameObject, startPos, hideBoardTime).setEase(animationCurve);
    }
}
