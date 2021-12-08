using UnityEngine;

public class ParkingSpotController : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Renderer renderer;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        renderer = gameObject.GetComponent<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Car"))
        {
            return;
        }

        var contains = boxCollider.bounds.Contains(other.bounds.max) && boxCollider.bounds.Contains(other.bounds.min);

        if (contains)
        {
            ChangeColor(Color.green);
        }
        else
        {
            ChangeColor(Color.red);
        }
    }

    private void ChangeColor(Color color)
    {
        renderer.material.color = color;
    }
}
