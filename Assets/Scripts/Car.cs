using TMPro;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected WheelsController wheelsController;
    [SerializeField] protected SensorsController sensorsController;
    [SerializeField] protected DistancesController distancesController;
    [SerializeField] protected BoxCollider carCollider;
    [SerializeField] protected TextMeshProUGUI lossText;
    [SerializeField] protected bool isLossVisible;
    [SerializeField] private bool isFitnessDistance;

    protected BoxCollider parkingSpotCollider;
    protected Transform parkingSportTransform;

    protected float loss;
    protected float startLossValue;

    private void Awake()
    {
        parkingSpotCollider = GameObject.FindWithTag("ParkingSpot").GetComponentInChildren<BoxCollider>();
        parkingSportTransform = parkingSpotCollider.transform;
    }

    protected float GetLoss()
    {
        if (isFitnessDistance)
        {
            return distancesController.GetDistanceTo(parkingSportTransform.position);
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
