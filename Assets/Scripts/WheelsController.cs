using UnityEngine;

internal enum CarDriveType
{
    FrontWheelDrive,
    RearWheelDrive,
    FourWheelDrive
}

internal enum SpeedType
{
    KPH,
    MPH
}

public class WheelsController : MonoBehaviour
{
    [SerializeField] private CarDriveType carDriveType;
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private GameObject[] wheelMeshes = new GameObject[4];
    [SerializeField] private Vector3 centerOfMass;
    [SerializeField] private float steerAngle = 50f;
    [SerializeField] private float driveTorque = 700;
    [SerializeField] private float brakeTorque = 700;
    [SerializeField] private float downForce = 100f;
    [SerializeField] private SpeedType speedType;
    [SerializeField] private float topSpeed = 20;

    private Rigidbody rb;

    public float CurrentSpeed { get { return rb.velocity.magnitude * 2.23693629f; } }

    public float MaxSpeed { get { return topSpeed; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        wheelColliders[0].attachedRigidbody.centerOfMass = centerOfMass;
    }
    public void Move(float acceleration, float steering, float braking)
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quat);
            wheelMeshes[i].transform.position = position;
            wheelMeshes[i].transform.rotation = quat;
        }

        braking = Mathf.Clamp(braking, 0f, 1f);

        var brake = braking * brakeTorque;
        foreach (var collider in wheelColliders)
        {
            collider.brakeTorque = brake;
        }

        steering = Mathf.Clamp(steering, -1, 1);

        var steerAngle = steering * this.steerAngle;
        wheelColliders[0].steerAngle = steerAngle;
        wheelColliders[1].steerAngle = steerAngle;

        acceleration = Mathf.Clamp(acceleration, -1, 1);

        ApplyDrive(acceleration);
        AddDownForce();
        CapSpeed();
    }
    private void ApplyDrive(float accel)
    {
        float thrustTorque;
        switch (carDriveType)
        {
            case CarDriveType.FourWheelDrive:
                thrustTorque = accel * (driveTorque / 4f);
                for (int i = 0; i < 4; i++)
                {
                    wheelColliders[i].motorTorque = thrustTorque;
                }
                
                break;

            case CarDriveType.FrontWheelDrive:
                thrustTorque = accel * (driveTorque / 2f);
                wheelColliders[0].motorTorque = wheelColliders[1].motorTorque = thrustTorque;
                break;

            case CarDriveType.RearWheelDrive:
                thrustTorque = accel * (driveTorque / 2f);
                wheelColliders[2].motorTorque = wheelColliders[3].motorTorque = thrustTorque;
                break;
        }
        rb.AddForce(transform.forward * 2f * driveTorque * accel);
    }
    private void AddDownForce()
    {
        wheelColliders[0].attachedRigidbody.AddForce(-transform.up * downForce * wheelColliders[0].attachedRigidbody.velocity.magnitude);
    }
    private void CapSpeed()
    {
        float speed = rb.velocity.magnitude;
        switch (speedType)
        {
            case SpeedType.MPH:
                speed *= 2.23693629f;
                if (speed > topSpeed)
                    rb.velocity = (topSpeed / 2.23693629f) * rb.velocity.normalized;
                break;

            case SpeedType.KPH:
                speed *= 3.6f;
                if (speed > topSpeed)
                    rb.velocity = (topSpeed / 3.6f) * rb.velocity.normalized;
                break;
        }
    }
}
