using UnityEngine;

public class PopulationHolder : MonoBehaviour
{
    public Transform GetFirstCarTransform()
    {
        var cars = transform.GetComponentsInChildren<DistanceToDestiantion>();

        Transform firstCar = cars[0].transform;

        float currLowestDistance = Mathf.Infinity;

        foreach (var car in cars)
        {
            if(car.GetDistanceToDestination() < currLowestDistance)
            {
                firstCar = car.transform;
                currLowestDistance = car.GetDistanceToDestination();
            }
        }

        return firstCar;
    }
}
