using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SensorsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sensor sensorPrefab;
    [SerializeField] private Transform sensorsParent;

    [Header("Sensors Settings")]
    [SerializeField] private Color colorOfSensors = Color.red;
    [SerializeField] private int lengthOfSensors = 8;
    [SerializeField] private float forwardSensorOffset = 0.8f;
    [SerializeField] private int angle = 180;

    [Header("Options")]
    [SerializeField] private bool drawRays = false;

    private List<Sensor> sensors;

    private TrainingManager trainingManager;

    private void Awake()
    {
        sensors = new List<Sensor>();
        trainingManager = FindObjectOfType<TrainingManager>();
    }

    void Start()
    {
        int sensorsCount = trainingManager.Inputs;

        int angleBetweenSensors = angle / (sensorsCount - 1);

        for (int i = 0; i < sensorsCount; i++)
        {
            Sensor sensor = Instantiate(sensorPrefab, sensorsParent);

            sensor.RototateByDegrees(i * angleBetweenSensors);
            sensor.MoveSensorForward(forwardSensorOffset);
            sensor.SetSensorColor(colorOfSensors);
            sensor.SetSensorLength(lengthOfSensors);
            sensor.ShouldDrawRay(drawRays);

            sensors.Add(sensor);
        }
    }

    public List<double> GetDistances()
    {
        return sensors.Select(x => x.GetDistanceToWall()).ToList();
    }
}
