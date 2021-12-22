﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SensorsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sensor sensorPrefab;
    [SerializeField] private Transform sensorsParent;

    [Header("Sensors Settings")]
    [SerializeField] private Color colorOfSensors;
    [SerializeField] private int lengthOfSensors;
    [SerializeField] private float forwardSensorOffset;

    [Header("Options")]
    [SerializeField] private bool drawRays;

    private List<Sensor> sensors;

    private void Awake()
    {
        sensors = new List<Sensor>();
    }

    void Start()
    {
        int sensorsCount = TrainingManager.Instance.Inputs;

        int angleBetweenSensors = 360 / sensorsCount;

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

    public float[] GetDistances()
    {
        return sensors.Select(x => x.GetDistanceToWall()).ToArray();
    }
}