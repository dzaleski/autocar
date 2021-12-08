using UnityEngine;

public class CarContoller : MonoBehaviour
{
    [SerializeField] private WheelsController wheelsController;

    private void Update()
    {
        Accelerate();
        Steer();
        Brake();
    }

    private void Accelerate()
    {
        float horizontalValue = Input.GetAxis("Vertical");
        wheelsController.Accelerate(horizontalValue);
    }

    private void Steer()
    {
        float verticalValue = Input.GetAxis("Horizontal");
        wheelsController.SteerWheels(verticalValue);
    }

    private void Brake()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            wheelsController.Brake(1);
        }
        else
        { 
            wheelsController.Brake(0);
        }
    }
}
