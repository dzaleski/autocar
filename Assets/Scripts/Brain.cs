using UnityEngine;

public abstract class Brain : MonoBehaviour
{
    public bool Disabled { get; set; }
    public NeuralNetwork neuralNetwork { get; set; }

    public void Initialise(NeuralNetwork neuralNetwork, Transform startPoint)
    {
        this.neuralNetwork = neuralNetwork;
        var newPos = new Vector3(startPoint.transform.position.x, 2.7f, startPoint.transform.position.z);
        transform.SetPositionAndRotation(newPos, startPoint.transform.rotation);

        OnInitialise();
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

    public abstract void OnInitialise();
}
