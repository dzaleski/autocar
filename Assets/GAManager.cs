using UnityEngine;

public class GAManager : MonoBehaviour
{
    [Header("GA Settings")]
    [SerializeField] private int populationSize;
    [Range(0.0f, 1.0f)] private int mutationProb;

    [Header("References")]
    [SerializeField] private Transform populationParent;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Car individualCar;

    private Car[] population;

    private void Start()
    {
        CreateInitialPopulation();
    }

    public void CreateInitialPopulation()
    {
        var population = new Car[populationSize];

        for (int i = 0; i < populationSize; i++)
        {
            Car car = Instantiate(individualCar, populationParent);
            car.transform.position = startPosition.position;
            population[i] = car;
        }
    }
}
