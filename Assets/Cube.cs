using Assets.Enums;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DistancesController))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : Brain
{
    [Header("Move Controls")]
    [SerializeField] private float moveForwardMultiplier = 20;
    [SerializeField] private float rotationMultiplier = 20;

    [Header("Raycasts Controls")]
    [SerializeField] private float raycastLength = 8f;
    [SerializeField] private bool drawRays = true;

    [Header("Idle Controls")]
    [SerializeField] private float secondsUntilCheck = 3;
    [SerializeField] private float distanceNeedToTravel = 10;

    private DistancesController distancesController;
    private Rigidbody rb;
    private BoxCollider boxCollider;

    private float halfOfWidth;
    private float halfOfLength;

    private void Awake()
    {
        distancesController = GetComponent<DistancesController>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
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
        Move(outputs);
    }

    private void Move(double[] outputs)
    {
        var movingForwardMultiplier = (float)outputs[0];
        var steerMultiplier = (float)outputs[1];

        if(outputs[2] >= 0)
        {
            movingForwardMultiplier = 0;
        }

        rb.MovePosition(rb.position + transform.forward * movingForwardMultiplier * moveForwardMultiplier * Time.deltaTime);
        rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + Vector3.up * steerMultiplier * rotationMultiplier * Time.deltaTime));
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

        Physics.Raycast(transform.position + movedTo, direction, out hit, raycastLength, wallMask);

        if(drawRays)
        {
            Debug.DrawRay(transform.position + movedTo, direction * raycastLength, Color.red);
        }

        if (hit.collider == null)
        {
            return 1;
        }

        return hit.distance / raycastLength;
    }

    public void DisableBrain()
    {
        float distanceLeft = distancesController.GetDistanceFromBrainToDestination();
        DecreaseScoreBy(distanceLeft);

        transform.gameObject.SetActive(false);
        Disabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
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

