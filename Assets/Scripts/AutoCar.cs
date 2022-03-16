using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WheelsController))]
[RequireComponent(typeof(Rigidbody))]
public class AutoCar : Car
{
    [HideInInspector] public bool Disabled { get; set; }
    [HideInInspector] public NeuralNetwork NeuralNetwork { get; set; }
    [HideInInspector] public Transform ParkingSpot { get; set; }

    [SerializeField] private float secondsUntilCheck = 3;
    [SerializeField] private float distanceNeedToTravel = 10;

    private BoxCollider parkingSpotCollider;
    private float loss;
    private float startLossValue;

    private void Start()
    {
        startLossValue = GetLoss();
        StartCoroutine(KillIfIdleCoroutine());
    }

    private void Update()
    {
        if(Disabled) return;

        loss = GetLoss();
        SetLossText();

        var inputs = sensorsController.GetInputs();
        var outputs = NeuralNetwork.Process(inputs);
        MoveCar(outputs);
    }

    private void MoveCar(float[] outputs)
    {
        float accelerateMultiplier = outputs[0];
        float steerMultiplier = outputs[1];
        float brakeMultiplier = 0f;

        if (loss <= 4f)
        {
            brakeMultiplier = 1f;
        }

        wheelsController.Move(accelerateMultiplier, steerMultiplier, brakeMultiplier);
    }

    public void Disable()
    {
        NeuralNetwork.Fitness = 1 / loss;
        Disabled = true;
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

            if (Vector3.Distance(prevPosition, currPosition) < distanceNeedToTravel)
            {
                Disable();
            }
        }
    }

    public void SetNeuralNetwork(NeuralNetwork neuralNetwork)
    {
        NeuralNetwork = neuralNetwork;
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
        if (isFitnessDistance)
        {
            return distancesController.GetDistanceTo(ParkingSpot.position);
        }

        return distancesController.GetAvgDistanceBetweenVertices(carCollider, parkingSpotCollider);
    }
}
