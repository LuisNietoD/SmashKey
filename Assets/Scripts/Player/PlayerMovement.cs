using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    public float speed;
    public Collider boundaryCollider;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 newVelocity = new Vector3(horizontal, 0, vertical).normalized * speed;
        _rb.linearVelocity = newVelocity;
    }

    void FixedUpdate()
    {
        if (boundaryCollider != null)
        {
            Vector3 clampedPosition = ClampToBoundary(transform.position);
            _rb.position = clampedPosition;
        }
    }

    private Vector3 ClampToBoundary(Vector3 position)
    {
        Bounds bounds = boundaryCollider.bounds;

        float clampedX = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
        float clampedY = position.y; // Keep the Y-axis unchanged (for 3D movement)
        float clampedZ = Mathf.Clamp(position.z, bounds.min.z, bounds.max.z);

        return new Vector3(clampedX, clampedY, clampedZ);
    }
}