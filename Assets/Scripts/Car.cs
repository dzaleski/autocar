using Assets.Enums;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WheelsController))]
[RequireComponent(typeof(DistancesController))]
[RequireComponent(typeof(Rigidbody))]
public class Car : Brain
{
    [Header("Raycasts Controls")]
    [SerializeField] private float raycastLength = 8f;
    [SerializeField] private float raisedBy = 1.2f;
    [SerializeField] private bool drawRays = false;

    [Header("Move Controls")]
    [SerializeField] private bool steerByAI = true;

    [Header("Idle Controls")]
    [SerializeField] private float secondsUntilCheck = 3;
    [SerializeField] private float distanceNeedToTravel = 10;

    private DistancesController distancesController;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private WheelsController wheelsController;

    private float halfOfWidth;
    private float halfOfLength;

    private void Awake()
    {
        wheelsController = GetComponent<WheelsController>();
        distancesController = GetComponent<DistancesController>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponentInChildren<BoxCollider>();

        halfOfWidth = boxCollider.bounds.extents.z;
        halfOfLength = boxCollider.bounds.extents.x;
    }

    private void Start()
    {
        StartCoroutine(KillIfIdleCoroutine());
    }

    private void FixedUpdate()
    {
        var inputs = GetInputs();
        var outputs = neuralNetwork.Process(inputs);

        if(steerByAI)
        {
            SteerByAI(outputs);
        }
        else
        {
            SteerByPlayer();
        }
    }

    private void SteerByAI(double[] outputs)
    {
        float movingForwardMultiplier = (float)outputs[0];
        float steerMultiplier = (float)outputs[1];
        float brakeMultiplier = (float)outputs[2];

        wheelsController.Accelerate(movingForwardMultiplier);
        wheelsController.SteerWheels(steerMultiplier);

        if(brakeMultiplier >= 0)
        {
            wheelsController.Brake(1f);
        }
        else
        {
            wheelsController.Brake(0f);
        }
        
    }

    private void SteerByPlayer()
    {
        wheelsController.SteerWheels(Input.GetAxis("Horizontal"));
        wheelsController.Accelerate(Input.GetAxis("Vertical"));
        wheelsController.Brake(Input.GetKey(KeyCode.Space) ? 1 : 0);
    }


    private double[] GetInputs()
    {
        var distances = new double[NeuralNetwork.Inputs];

        var directionEnumValues = Enum.GetValues(typeof(Direction));

        for (int i = 0; i < directionEnumValues.Length; i++)
        {
            distances[i] = GetDistanceFrom((Direction)directionEnumValues.GetValue(i));
        }

        return distances;
    }

    private float GetDistanceFrom(Direction direciton)
    {
        var toRightSide = transform.right * halfOfWidth;
        var toFrontSide = transform.forward * halfOfLength;

        switch (direciton)
        {
            case Direction.Left:
                return GetDistanceFromRaycast(toRightSide, transform.right);
            case Direction.Right:
                return GetDistanceFromRaycast(-toRightSide, -transform.right);
            case Direction.Forward:
                return GetDistanceFromRaycast(toFrontSide, transform.forward);
            case Direction.Backward:
                return GetDistanceFromRaycast(-toFrontSide, -transform.forward);
            case Direction.ForwardLeft:
                return GetDistanceFromRaycast(toFrontSide + toRightSide, Quaternion.AngleAxis(45f, transform.up) * transform.forward);
            case Direction.ForwardRight:
                return GetDistanceFromRaycast(toFrontSide - toRightSide, Quaternion.AngleAxis(-45f, transform.up) * transform.forward);
            case Direction.BackwardRight:
                return GetDistanceFromRaycast(-toFrontSide - toRightSide, Quaternion.AngleAxis(45f, transform.up) * -transform.forward);
            default:
                return GetDistanceFromRaycast(-toFrontSide + toRightSide, Quaternion.AngleAxis(-45f, transform.up) * -transform.forward);
        }
    }


    private float GetDistanceFromRaycast(Vector3 movedTo, Vector3 direction)
    {
        int wallMask = 1 << 9;
        RaycastHit hit;

        var raycastOrigin = transform.position + movedTo + (transform.up * raisedBy);

        Physics.Raycast(raycastOrigin, direction, out hit, raycastLength, wallMask);

        if (drawRays)
        {
            Debug.DrawRay(raycastOrigin, direction * raycastLength, Color.red);
        }

        if (hit.collider == null)
        {
            return 1;
        }

        return Mathf.Clamp01(hit.distance / raycastLength);
    }

    public void DisableBrain()
    {
        float distanceLeft = distancesController.GetDistanceFromBrainToDestination();
        DecreaseScoreBy(distanceLeft);
        SetCarAsDisabled();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
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
}
