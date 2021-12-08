using Assets.Scripts;
using System;
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
        Bias = UnityEngine.Random.Range(-1, 1);
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
}
