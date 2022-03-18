using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(WheelsController))]
[RequireComponent(typeof(SensorsController))]
[RequireComponent(typeof(DistancesController))]
[RequireComponent(typeof(Rigidbody))]
public class AutoCar : MonoBehaviour
{
    [SerializeField] private float percentAccuaryToStop = 0.02f;
    [SerializeField] private float secondsUntilCheck = 3;
    [SerializeField] private float distanceNeedToTravel = 10;
    [SerializeField] private bool drivingByPlayer;
    [SerializeField] protected TextMeshProUGUI lossText;

    public bool IsDisabled => disabled;

    private WheelsController wheelsController;
    private SensorsController sensorsController;
    private DistancesController distancesController;
    private Transform parkingSpot;
    private NeuralNetwork neuralNetwork;
    private float loss;
    private float startLossValue;
    private bool disabled;
    private bool isIdling;

    private void Awake()
    {
        wheelsController = GetComponent<WheelsController>();
        sensorsController = GetComponent<SensorsController>();
        distancesController = GetComponent<DistancesController>();
    }

    private void Start()
    {
        startLossValue = GetLoss();
        if(!drivingByPlayer) StartCoroutine(KillIfIdleCoroutine());
    }

    private void Update()
    {
        if (disabled) return;
        if (isIdling) Disable();

        loss = GetLoss();
        SetLossText();

        var inputs = sensorsController.GetInputs();
        var outputs = neuralNetwork.Process(inputs);
        MoveCar(outputs);
    }

    private void MoveCar(float[] outputs)
    {
        if (drivingByPlayer)
        {
            wheelsController.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.Space) ? 1f : 0f);
            return;
        }

        float accelerateMultiplier = outputs[0];
        float steerMultiplier = outputs[1];
        float brakeMultiplier = 0f;

        if (loss <= startLossValue * percentAccuaryToStop)
        {
            brakeMultiplier = 1f;
        }

        wheelsController.Move(accelerateMultiplier, steerMultiplier, brakeMultiplier);
    }

    public void Disable()
    {
        neuralNetwork.Fitness = 1 / loss;
        disabled = true;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Car"))
        {
            Disable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ParkingArea"))
        {
            Disable();
        }
    }

    private IEnumerator KillIfIdleCoroutine()
    {
        while (true)
        {
            var prevPosition = transform.position;
            yield return new WaitForSeconds(secondsUntilCheck);
            var currPosition = transform.position;
            isIdling = Vector3.Distance(prevPosition, currPosition) < distanceNeedToTravel;
        }
    }

    public void SetNeuralNetwork(NeuralNetwork network)
    {
        neuralNetwork = network;
    }

    public void SetParkingSpot(Transform spot)
    {
        parkingSpot = spot;
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

    public float GetLoss()
    {
        return distancesController.GetDistanceTo(parkingSpot.position);
    }
}
