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

    [SerializeField] private bool isLossVisible;
    [SerializeField] private float secondsUntilCheck = 3;
    [SerializeField] private float distanceNeedToTravel = 10;

    [SerializeField] private WheelsController wheelsController;
    [SerializeField] private SensorsController sensorsController;
    [SerializeField] private DistancesController distancesController;
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
        startLoss = distancesController.GetAvgDistanceBetweenVertices(carCollider, parkingSpotCollider);
        StartCoroutine(KillIfIdleCoroutine());
    }

    private void Update()
    {
        loss = distancesController.GetAvgDistanceBetweenVertices(carCollider, parkingSpotCollider);
        SetLossText(loss);

        var inputs = sensorsController.GetInputs();
        var outputs = NeuralNetwork.Process(inputs);
        MoveCar(outputs);
    }

    private void MoveCar(float[] outputs)
    {
        float accelerateMultiplier = outputs[0].Sigmoid() <= .5f ? 1f : -1f;
        float steerMultiplier = outputs[1];
        float brakeMultiplier = outputs[2];

        if (loss < 1f)
        {
            brakeMultiplier = 1f;
        }

        wheelsController.Move(accelerateMultiplier, steerMultiplier, brakeMultiplier);
    }

    public void DisableBrain()
    {
        CalculteFitness();
        SetCarAsDisabled();
    }

    private void CalculteFitness()
    {
        NeuralNetwork.Fitness = 1 / loss;
    }

    private float GetAvgDistanceBetweenVertices(Vector3[] fromVertices, Vector3[] toVertices)
    {
        float sumDistances = 0;
        int verticesCount = fromVertices.Length;

        for (int i = 0; i < verticesCount; i++)
        {
            sumDistances += Vector2.Distance(fromVertices[i].ToVector2XZ(), toVertices[i].ToVector2XZ()) * 10f;
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

        lossText.text = $"{roundedLoss}";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            DisableBrain();
        }
        else if (collision.collider.CompareTag("Car"))
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
