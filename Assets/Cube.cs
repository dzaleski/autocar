using UnityEngine;

public class Cube : Brain
{
    [Header("Move Controls")]
    [SerializeField] private float moveForwardMultiplier = 20;
    [SerializeField] private float rotationMultiplier = 20;

    private DistancesController distancesController;
    private Rigidbody rb;

    private float currentTime;
    private float previousDistance;
    private float secondsToCheck = 2;
    private float distanceToTravel = 1;

    private void Awake()
    {
        distancesController = GetComponent<DistancesController>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        KillIfIdle();

        var inputs = GetInputs();

        (float accelerationMultiplier, float steerMultiplier) = neuralNetwork.Process(inputs);

        Move(accelerationMultiplier, steerMultiplier);
    }

    private void Move(float accelerationMultiplier, float steerMultiplier)
    {
        rb.MovePosition(rb.position + transform.forward * accelerationMultiplier * moveForwardMultiplier * Time.deltaTime);
        rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + Vector3.up * steerMultiplier * rotationMultiplier * Time.deltaTime));
    }

    private double[] GetInputs()
    {
        int angleCoveredByRaycasts = 270;
        int inputsCount = NeuralNetwork.Inputs;
        var inputs = new double[inputsCount];

        int angleBetweenRaycasts = angleCoveredByRaycasts / inputsCount;

        int raycastsCount = NeuralNetwork.Inputs - 1;

        inputs[0] = GetDistanceToWallFromRay(0);

        int inputsIndex = 1;

        for (int i = 1; i <= raycastsCount / 2; i++)
        {
            inputs[inputsIndex] = GetDistanceToWallFromRay(i * angleBetweenRaycasts);
            inputsIndex++;
            inputs[inputsIndex] = GetDistanceToWallFromRay(i * -angleBetweenRaycasts);
            inputsIndex++;
        }

        return inputs;
    }

    private double GetDistanceToWallFromRay(int rayRotation)
    {
        int wallMask = 1 << 9;
        float raycastLength = 16f;

        var rotatedDirection = Quaternion.AngleAxis(rayRotation, transform.up) * transform.forward;

        RaycastHit hit;
        Physics.Raycast(transform.position, rotatedDirection, out hit, raycastLength, wallMask);

        //Debug.DrawRay(transform.position, rotatedDirection * raycastLength, Color.red);

        if (hit.collider == null)
        {
            return 1;
        }

        return hit.distance / raycastLength;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var wall = collision.collider.GetComponent<Wall>();

        if (wall != null)
        {
            DisableCar();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var destination = other.GetComponent<Destination>();

        if (destination != null)
        {
            Score += 80;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var destination = other.GetComponent<Destination>();

        if(destination != null)
        {
            Score += 10;
        }
    }

    private void DisableCar()
    {
        transform.gameObject.SetActive(false);
        Score += distancesController.GetTraveledDistance();
        Disabled = true;
    }

    private void KillIfIdle()
    {
        if (currentTime == 0)
        {
            previousDistance = distancesController.GetTraveledDistance();
        }

        currentTime += Time.deltaTime;

        if (currentTime < secondsToCheck) return;

        currentTime = 0;

        float currentDistance = distancesController.GetTraveledDistance();
        float differenceBetweenDistances = Mathf.Abs(currentDistance - previousDistance);

        if (differenceBetweenDistances < distanceToTravel)
        {
            DisableCar();
        }
    }
}
