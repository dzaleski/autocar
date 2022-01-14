using UnityEngine;

public class WheelsController : MonoBehaviour
{
    [SerializeField] private WheelCollider frontDriverW;
    [SerializeField] private WheelCollider frontPassengerW;
    [SerializeField] private WheelCollider backDriverW;
    [SerializeField] private WheelCollider backPassengerW;

    [SerializeField] private Transform frontDriverT;
    [SerializeField] private Transform frontPassengerT;
    [SerializeField] private Transform backDriverT;
    [SerializeField] private Transform backPassengerT;

    [SerializeField] private float maxSteerAngle = 45;
    [SerializeField] private float maxDriveForce = 400;

    private float _steer;
    private float _acceleration;
    private float _brakeValue;

    public void SteerWheels(float horizontalValue)
    {
        _steer = horizontalValue;
    }

    public void Accelerate(float accelerationMultiplier)
    {
        _acceleration = accelerationMultiplier;
    }

    public void Brake(float brakeValue)
    {
        _brakeValue = brakeValue;
    }

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
        Brake();
        UpdateWheels();
    }

    private void Steer()
    {
        frontDriverW.steerAngle = maxSteerAngle * _steer;
        frontPassengerW.steerAngle = maxSteerAngle * _steer;
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = maxDriveForce * _acceleration;
        frontPassengerW.motorTorque = maxDriveForce * _acceleration;
    }

    private void Brake()
    {
        frontDriverW.brakeTorque = maxDriveForce * _brakeValue;
        frontPassengerW.brakeTorque = maxDriveForce * _brakeValue;
        backDriverW.brakeTorque = maxDriveForce * _brakeValue;
        backPassengerW.brakeTorque = maxDriveForce * _brakeValue;
    }

    private void UpdateWheels()
    {
        UpdateWheel(backDriverW, backDriverT);
        UpdateWheel(backPassengerW, backPassengerT);
        UpdateWheel(frontDriverW, frontDriverT);
        UpdateWheel(frontPassengerW, frontPassengerT);
    }

    private void UpdateWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion quat;

        wheelCollider.GetWorldPose(out pos, out quat);

        wheelTransform.position = pos;
        wheelTransform.rotation = quat;
    }
}
