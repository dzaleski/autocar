using Assets.Scripts.Extensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(WheelsController))]
[RequireComponent(typeof(SensorsController))]
[RequireComponent(typeof(DistancesController))]
[RequireComponent(typeof(Rigidbody))]
public class AutoCar : MonoBehaviour
{
    [SerializeField] private float accuaryToStop = 1.5f;
    [SerializeField] private float secondsUntilCheck = 3;
    [SerializeField] private float distanceNeedToTravel = 10;
    [SerializeField] private bool drivingByPlayer;
    [SerializeField] protected TextMeshProUGUI lossText;

    public bool IsDisabled => disabled;
    public bool IsTesting;

    public Action OnDisable;

    private WheelsController wheelsController;
    private SensorsController sensorsController;
    private DistancesController distancesController;

    private NeuralNetwork neuralNetwork;

    private float loss;
    private float startLossValue;
    private bool disabled;
    private bool isIdling;
    private bool isInParkingSpot;
    private float bonusPoints;

    private void Awake()
    {
        wheelsController = GetComponent<WheelsController>();
        sensorsController = GetComponent<SensorsController>();
        distancesController = GetComponent<DistancesController>();
    }

    private void Start()
    {
        if (!drivingByPlayer)
        {
            StartCoroutine(KillIfIdleCoroutine());
        }
    }

    private void Update()
    {
        if (disabled)
        {
            return;
        }
        
        if (isIdling) 
        {
            if(isInParkingSpot)
            {
                bonusPoints = .5f;
                Debug.Log("Added bouns points");
            }
            Disable();
            bonusPoints = 0f;
        };

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
        float brakeMultiplier = outputs[2] <= 0f ? 0f : 1f;

        wheelsController.Move(accelerateMultiplier, steerMultiplier, brakeMultiplier);
    }

    public void Disable()
    {
        neuralNetwork.Fitness = 100 * (1 / loss) + bonusPoints;
        disabled = true;
        sensorsController.HideSensors();
        GetComponentInChildren<Rigidbody>().isKinematic = true;

        if(OnDisable != null)
        {
            OnDisable.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Car"))
        {
            bonusPoints = -0.3f;
            Disable();
            bonusPoints = 0;
        }
        if(collision.collider.CompareTag("ParkingSpot"))
        {
            isInParkingSpot = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ParkingSpot"))
        {
            isInParkingSpot = false;
        }
    }

    private IEnumerator KillIfIdleCoroutine()
    {
        while (true && !IsTesting)
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
        distancesController.SetParkingSpotAndItsCollider(spot);
    }

    public void SetStartLossValue()
    {
        startLossValue = GetLoss();
    }

    protected void SetLossText()
    {
        lossText.color = GetLossTextColor();
        lossText.text = GetLoss().ToString("#0.0");
    }

    private Color GetLossTextColor()
    {
        if (loss <= startLossValue * 0.1f) return Color.green;
        if (loss <= startLossValue * 0.4f) return Color.yellow;
        return Color.red;
    }

    public float GetLoss()
    {
        var distanceLeft = distancesController.GetDistanceToParkingSpot();
        var fitIntoSpot = distancesController.GetFitIntoParkingSpot();

        return distanceLeft + fitIntoSpot;
    }

    public float GetFitness()
    {
        return Mathf.Clamp(startLossValue - loss, 0f, startLossValue); ;
    }
}
