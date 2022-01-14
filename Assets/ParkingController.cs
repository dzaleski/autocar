using UnityEngine;

public class ParkingController : MonoBehaviour
{
    public void RestartParkedCars()
    {
        var cars = GetComponentsInChildren<ParkedCar>();

        foreach (var car in cars)
        {
            car.RestartPosition();
        }
    }
}
