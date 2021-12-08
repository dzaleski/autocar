using System.Collections.Generic;

namespace Assets.Scripts.Car
{
    public class Layer
    {
        public List<Neuron> Neurons;

        public Layer(List<Neuron> neurons)
        {
            Neurons = neurons;
        }
    }
}
