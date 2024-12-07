using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionPrefab;
    private float explosionDelay;
    private float explosionRadius;

    public void Initialize(float delay, float radius)
    {
        explosionDelay = delay;
        explosionRadius = radius;
        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hitColliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 explosionDirection = (rb.transform.position - transform.position).normalized;
                rb.AddForce(explosionDirection * 10f, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}