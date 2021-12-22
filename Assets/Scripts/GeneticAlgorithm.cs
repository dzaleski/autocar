using System.Linq;

public class GeneticAlgorithm
{
    private int populationSize;
    private float mutationProb;
    private int numberOfParents;

    private NeuralNetwork[] population;

    public GeneticAlgorithm(int populationSize, float mutationProb, int numberOfParents)
    {
        this.populationSize = populationSize;
        this.mutationProb = mutationProb;
        this.numberOfParents = numberOfParents;

        population = new NeuralNetwork[populationSize];
    }

    public NeuralNetwork[] CreateInitialPopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            population[i] = new NeuralNetwork();
        }

        return population;
    }

    public NeuralNetwork[] Reproduce()
    {
        NeuralNetwork[] parents = GetParents();

        population = new NeuralNetwork[populationSize];

        for (int i = 0; i < populationSize; i++)
        {
            population[i] = new NeuralNetwork(parents, mutationProb);
        }

        return population;
    }

    private NeuralNetwork[] GetParents()
    {
        return population.OrderByDescending(x => x.Score).Take(numberOfParents).ToArray();
    }
}
