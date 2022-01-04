using Assets.Scripts;
using System.Collections.Generic;

public class Neuron
{
    public float Value;
    public float Bias;
    public List<Synapse> incomeSynapses;
    public List<Synapse> outcomeSynapses;

    public Neuron()
    {
        incomeSynapses = new List<Synapse>();
        outcomeSynapses = new List<Synapse>();
        Bias = UnityEngine.Random.Range(-1f, 1f);
    }

    public Neuron(float value) : this()
    {
        Value = value;
    }

    public Neuron(List<Neuron> inputNeurons) : this()
    {
        foreach (Neuron inputNeuron in inputNeurons)
        {
            var synapse = new Synapse(inputNeuron, this);
            inputNeuron.outcomeSynapses.Add(synapse);
            incomeSynapses.Add(new Synapse(inputNeuron, this));
        }
    }

    public List<Synapse> GetOutcomeSynapses()
    {
        return outcomeSynapses;
    }
}
