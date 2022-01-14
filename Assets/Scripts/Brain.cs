using UnityEngine;

public abstract class Brain : MonoBehaviour
{
    public bool Disabled { get; set; }
    public NeuralNetwork neuralNetwork { get; set; }

    public void Initialise(NeuralNetwork neuralNetwork, Vector3 startPos)
    {
        this.neuralNetwork = neuralNetwork;
        var newPos = new Vector3(startPos.x, transform.position.y, startPos.z);
        transform.SetPositionAndRotation(newPos, transform.parent.rotation);
    }

    public void IncreaseScoreBy(float amount)
    {
        neuralNetwork.Score += amount;
    }

    public void DecreaseScoreBy(float amount)
    {
        neuralNetwork.Score -= amount;
    }

    public void SetCarAsDisabled()
    {
        Disabled = true;
        transform.gameObject.SetActive(false);
    }

}
