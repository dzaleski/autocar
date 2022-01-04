using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Car car = collision.transform.GetComponent<Car>();

        if (car != null)
        {
            car.DisableCar();
        }
    }
}
