using System.Linq;
using UnityEngine;

public class PlayerCar : Car
{
    private void Update()
    {
        sensorsController.GetInputs().ToList(); //In order to see raycasts
        loss = GetLoss();
        SetLossText();
        MoveCar();
    }

    private void MoveCar() 
    { 
        wheelsController.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.Space) ? 1f : 0f);
    }
}
