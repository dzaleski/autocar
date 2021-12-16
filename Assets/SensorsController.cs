﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SensorsController : MonoBehaviour
{
    [SerializeField] private Sensor sensorPrefab;
    [SerializeField] private Transform sensorsParent;
    [SerializeField] private Color colorOfSensors;
    [SerializeField] private int lengthOfSensors;
    [SerializeField] private float forwardSensorOffset;

    [Range(10, 50)] int sensorLength;
    [Range(2, 15)] public int sensorsCount;

    private List<Sensor> sensors;
    public int Inputs => sensorsCount;

    private void Awake()
    {
        sensors = new List<Sensor>();
    }

    void Start()
    {
        int angleBetweenSensors = 360 / sensorsCount;

        for (int i = 0; i < sensorsCount; i++)
        {
            Sensor sensor = Instantiate(sensorPrefab, sensorsParent);
            sensor.RototateByDegrees(i * angleBetweenSensors);
            sensor.MoveSensorForward(forwardSensorOffset);
            sensor.SetSensorColor(colorOfSensors);
            sensor.SetSensorLength(lengthOfSensors);
            sensors.Add(sensor);
        }
    }

    public float[] GetDistances()
    {
        return sensors.Select(x => x.GetDistanceToWall()).ToArray();
    }
}