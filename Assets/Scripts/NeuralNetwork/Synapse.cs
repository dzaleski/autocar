using UnityEngine;

namespace Assets.Scripts
{
    public class Synapse
    {
        public Neuron InputNeuron;
        public Neuron OutputNeuron;
        private float weight;

        public float Weight => weight;

        public Synapse(Neuron inputNeuron, Neuron outputNeuron)
        {
            InputNeuron = inputNeuron;
            OutputNeuron = outputNeuron;
            weight = Random.Range(-1f, 1f);
        }

        public void SetWeight(float weight)
        {
            this.weight = weight;
        }

        public void SetRandomWeight()
        {
            weight = Random.Range(-1f, 1f);
        }
    }
}
