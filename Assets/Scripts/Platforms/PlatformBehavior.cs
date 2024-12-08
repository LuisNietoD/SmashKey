using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    public Transform front;
    [SerializeField]
    private MeshFilter _meshFilter;

    [SerializeField] private Rigidbody _rb;

    private void Update()
    {
        _rb.linearVelocity = new Vector3(0, 0, GameController.Metrics.platformTravelTime);
        if (transform.position.z < PlatformManager.Instance.platformEndPoint.position.z)
        {
            PlatformManager.Instance.InstantiateMap();
            Destroy(gameObject);
        }
    }
}
