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

    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float maxDriveForce;

    private float _horizontalValue;
    private float _verticalValue;
    private float _brakeValue;

    public void SteerWheels(float horizontalValue)
    {
        _horizontalValue = horizontalValue;
    }

    public void Accelerate(float verticalValue)
    {
        _verticalValue = verticalValue;
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
        frontDriverW.steerAngle = maxSteerAngle * _horizontalValue;
        frontPassengerW.steerAngle = maxSteerAngle * _horizontalValue;
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = maxDriveForce * _verticalValue;
        frontPassengerW.motorTorque = maxDriveForce * _verticalValue;
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
