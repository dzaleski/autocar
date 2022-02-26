using System;
using System.Collections;
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
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private WheelsController wheelsController;
    [SerializeField] private SensorsController sensorsController;

    private void Start()
    {
        StartCoroutine(KillIfIdleCoroutine());
    }

    private void Update()
    {
        var inputs = sensorsController.GetInputs();
        var outputs = NeuralNetwork.Process(inputs);
        MoveCar(outputs);
    }

    private void MoveCar(double[] outputs)
    {
        float movingForwardMultiplier = Sigmoid(outputs[0]);
        float steerMultiplier = (float)outputs[1];
        float brakeMultiplier = (float)outputs[2];

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
        //Set score
        SetCarAsDisabled();
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

    private static float Sigmoid(double value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value));
    }

    private void IncreaseScoreBy(float amount)
    {
        NeuralNetwork.Score += amount;
    }

    private void DecreaseScoreBy(float amount)
    {
        NeuralNetwork.Score -= amount;
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
