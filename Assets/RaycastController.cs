using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private List<Ray> raycasts;
    private int raycastLength = 8;

    private void Awake()
    {
        raycasts = new List<Ray>();

    }

    private void Start()
    {
    }

    private void Update()
    {
        DrawRays();
    }

    private void DrawRays()
    {
        raycasts.ForEach(r => Debug.DrawRay(r.origin, r.direction * raycastLength, Color.red));
    }

    public void InitialiseRaycasts()
    {
        int inputsCount = NeuralNetwork.Inputs;
        int angleBetweenRaycasts = 180 / inputsCount;

        int raycastsCount = inputsCount - 1;

        raycasts.Add(GetRaycastWithRotation(0));

        for (int i = 1; i <= raycastsCount / 2; i++)
        {
            raycasts.Add(GetRaycastWithRotation(i * angleBetweenRaycasts));
            raycasts.Add(GetRaycastWithRotation(i * -angleBetweenRaycasts));
        }
    }

    private Ray GetRaycastWithRotation(int rotationDegrees)
    {
        var rotatedDirection = Quaternion.AngleAxis(rotationDegrees, transform.up) * transform.forward;
        return new Ray(transform.localPosition, rotatedDirection);
    }

    public double[] GetInputs()
    {
        var inputs = new double[raycasts.Count];

        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = GetDistanceToWallFromRay(raycasts[i]);
        }

        return inputs;
    }

    private double GetDistanceToWallFromRay(Ray ray)
    {
        int wallMask = 1 << 9;

        RaycastHit hit;
        Physics.Raycast(ray, out hit, raycastLength, wallMask);

        if (hit.collider == null)
        {
            return 1;
        }

        Debug.Log($"Hitted {hit.collider.tag}");

        return hit.distance / raycastLength;
    }
}
