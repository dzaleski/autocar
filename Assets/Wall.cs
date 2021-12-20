using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Car"))
        {
            Destroy(collision.gameObject);
        }
    }
}
