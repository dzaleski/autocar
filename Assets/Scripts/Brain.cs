using UnityEngine;

public class Brain : MonoBehaviour
{
    public float Score { get; set; }
    public bool Disabled { get; set; }
    public NeuralNetwork neuralNetwork { get; set; }

    public void Initialise(NeuralNetwork neuralNetwork, Vector3 startPos)
    {
        this.neuralNetwork = neuralNetwork;

        var newPos = new Vector3(startPos.x, transform.position.y, startPos.z);
        transform.SetPositionAndRotation(newPos, transform.parent.rotation);
    }
}
