using Assets.Scripts.Extensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(WheelsController))]
[RequireComponent(typeof(Rigidbody))]
public class AutoCar : MonoBehaviour
{
    [HideInInspector] public bool Disabled;
    [HideInInspector] public NeuralNetwork NeuralNetwork;

    [Header("Idle Controls")]
    [SerializeField] private float secondsUntilCheck = 3;
    [SerializeField] private float distanceNeedToTravel = 10;

    [Header("References")]
    [SerializeField] private WheelsController wheelsController;
    [SerializeField] private SensorsController sensorsController;
    [SerializeField] private BoxCollider carCollider;
    [SerializeField] private TextMeshProUGUI lossText;

    private BoxCollider parkingSpotCollider;

    private float loss;

    private void Awake()
    {
        parkingSpotCollider = GameObject.FindWithTag("ParkingSpot").GetComponentInChildren<BoxCollider>();
    }

    private void Start()
    {
        StartCoroutine(KillIfIdleCoroutine());
    }

    private void Update()
    {
        var inputs = sensorsController.GetInputs();
        var outputs = NeuralNetwork.Process(inputs);
        MoveCar(outputs);

        SetLoss();
    }

    private void MoveCar(float[] outputs)
    {
        float movingForwardMultiplier = outputs[0];
        float steerMultiplier = outputs[1];
        float brakeMultiplier = outputs[2];

        wheelsController.Accelerate(movingForwardMultiplier);
        wheelsController.SteerWheels(steerMultiplier);

        if (brakeMultiplier >= 0)
        {
            wheelsController.Brake(1f);
        }
        else
        {
            wheelsController.Brake(0f);
        }
    }

    public void DisableBrain()
    {
        NeuralNetwork.Fitness = 1 / (loss + 0.1f);
        SetCarAsDisabled();
    }

    private void SetLoss()
    {
        var carVertices = carCollider.GetVertices();
        var parkingSportVertices = parkingSpotCollider.GetVertices();

        loss = GetAvgDistanceBetweenVertices(carVertices, parkingSportVertices);

        SetLossText(loss);
    }

    private float GetAvgDistanceBetweenVertices(Vector3[] fromVertices, Vector3[] toVertices)
    {
        float sumDistances = 0;
        int verticesCount = fromVertices.Length;

        for (int i = 0; i < verticesCount; i++)
        {
            sumDistances += Vector3.Distance(fromVertices[i], toVertices[i]);
        }

        return sumDistances / verticesCount;
    }

    private void SetLossText(float lossValue)
    {
        var roundedLoss = Math.Round(lossValue, 1);

        if (roundedLoss <= 1)
        {
            lossText.color = Color.green;
        }
        else if (roundedLoss < 3)
        {
            lossText.color = Color.yellow;
        }
        else
        {
            lossText.color = Color.red;
        }

        lossText.text = $"Loss: {roundedLoss}";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            DisableBrain();
        }
        else if (collision.collider.CompareTag("Car"))
        {
            //DecreaseScoreBy(5f);
            DisableBrain();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("ParkingArea"))
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

    private void IncreaseScoreBy(float amount)
    {
        NeuralNetwork.Fitness += amount;
    }

    private void DecreaseScoreBy(float amount)
    {
        NeuralNetwork.Fitness -= amount;
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
