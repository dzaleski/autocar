using System.Linq;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WheelsController wheelsController;
    [SerializeField] private SensorsController sensorsController;

    private void Update()
    {
        sensorsController.GetInputs().ToList(); //I order to see raycasts
        MoveCar();
    }

    private void MoveCar()
    {
        wheelsController.SteerWheels(Input.GetAxis("Horizontal"));
        wheelsController.Accelerate(Input.GetAxis("Vertical"));
        wheelsController.Brake(Input.GetKey(KeyCode.Space) ? 1 : 0);
    }

    public double GetDistance(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;

        Physics.Raycast(origin, direction, out hit, 10f);

        if (true)
        {
            Debug.DrawRay(origin, direction * 8f, Color.red);
        }

        if (hit.collider == null)
        {
            return 1;
        }

        return Mathf.Clamp01(hit.distance / 8f);
    }

}
