using TMPro;
using UnityEngine;

public class Car : MonoBehaviour
{
    [HideInInspector] public Transform ParkingSpot { get; set; }

    [Header("References")]
    [SerializeField] protected WheelsController wheelsController;
    [SerializeField] protected SensorsController sensorsController;
    [SerializeField] protected DistancesController distancesController;
    [SerializeField] protected BoxCollider carCollider;
    [SerializeField] protected TextMeshProUGUI lossText;
    [SerializeField] protected bool isLossVisible;
    [SerializeField] private bool isFitnessDistance;

    protected BoxCollider parkingSpotCollider;

    protected float loss;
    protected float startLossValue;

    private void Start()
    {
        parkingSpotCollider = ParkingSpot.GetComponentInChildren<BoxCollider>();
    }

    protected float GetLoss()
    {
        if (isFitnessDistance)
        {
            return distancesController.GetDistanceTo(ParkingSpot.position);
        }

        return distancesController.GetAvgDistanceBetweenVertices(carCollider, parkingSpotCollider);
    }

    protected void SetLossText()
    {
        lossText.color = GetLossTextColor();
        lossText.text = loss.ToString("#0.0");
    }

    private Color GetLossTextColor()
    {
        if (loss <= startLossValue * 0.1f) return Color.green;
        if (loss <= startLossValue * 0.4f) return Color.yellow;
        return Color.red;
    }
}
