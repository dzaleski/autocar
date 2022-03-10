using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WheelsController))]
[RequireComponent(typeof(Rigidbody))]
public class AutoCar : Car
{
    [HideInInspector] public bool Disabled { get; set; }
    [HideInInspector] public NeuralNetwork NeuralNetwork { get; set; }

    [SerializeField] private float secondsUntilCheck = 3;
    [SerializeField] private float distanceNeedToTravel = 10;


    private void Start()
    {
        startLossValue = GetLoss();
        StartCoroutine(KillIfIdleCoroutine());
    }

    private void Update()
    {
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

        if (loss <= startLossValue * 0.12f)
        {
            brakeMultiplier = 1f;
        }

        wheelsController.Move(accelerateMultiplier, steerMultiplier, brakeMultiplier);
    }

    public void DisableBrain()
    {
        NeuralNetwork.Fitness = 1 / loss;
        SetCarAsDisabled();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Car"))
        {
            DisableBrain();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ParkingArea"))
        {
            DisableBrain();
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
                DisableBrain();
            }
        }
    }

    private void SetCarAsDisabled()
    {
        Disabled = true;
        transform.gameObject.SetActive(false);
    }

    public void SetNeuralNetwork(NeuralNetwork neuralNetwork)
    {
        NeuralNetwork = neuralNetwork;
    }
}
