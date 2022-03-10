using UnityEngine;

public class TestEnvironmentController : MonoBehaviour
{
    [SerializeField] private TestAreaController testAreaPrefab;

    private MeshCollider meshCollider;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        var min = meshCollider.bounds.min;
        var max = meshCollider.bounds.max;

        

        var a = Instantiate(testAreaPrefab);
        a.transform.position = new Vector3(min.x, max.y, max.z);
    }

    public void InstantiateAreas()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 5; k++)
            {
                var area = Instantiate(testAreaPrefab, transform);
                area.transform.SetPositionAndRotation(new Vector3(), Quaternion.identity);
            }
        }
    }
}
