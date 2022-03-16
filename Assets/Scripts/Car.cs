using TMPro;
using UnityEngine;

[RequireComponent(typeof(WheelsController))]
[RequireComponent(typeof(SensorsController))]
[RequireComponent(typeof(DistancesController))]
public class Car : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] protected bool isLossVisible;
    [SerializeField] protected bool isFitnessDistance;
    [SerializeField] protected TextMeshProUGUI lossText;

    protected WheelsController wheelsController;
    protected SensorsController sensorsController;
    protected DistancesController distancesController;
    protected BoxCollider carCollider;

    private void Awake()
    {
        wheelsController = GetComponent<WheelsController>();
        sensorsController = GetComponent<SensorsController>();
        distancesController = GetComponent<DistancesController>();
        carCollider = GetComponentInChildren<BoxCollider>();
    }
}
