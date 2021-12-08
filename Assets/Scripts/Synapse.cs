using UnityEngine;

namespace Assets.Scripts
{
    public class Synapse
    {
        public Neuron InputNeuron;
        public Neuron OutputNeuron;
        public float Weight;

        public Synapse(Neuron inputNeuron, Neuron outputNeuron)
        {
            InputNeuron = inputNeuron;
            OutputNeuron = outputNeuron;
            Weight = Random.Range(-1, 1);
        }
    }
}
