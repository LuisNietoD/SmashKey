using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionPrefab;
    private float explosionRadius;
    private float yExplosion;

    public void Initialize(float delay, float radius)
    {
        yExplosion = delay;
        explosionRadius = radius;
        //Invoke(nameof(Explode), explosionDelay);
    }

    private void Update()
    {
        if (transform.position.y <= yExplosion)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, GameMetrics.Global.platformTravelTime);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hitColliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (hit.CompareTag("Enemy"))
                {
                    StatsManager.Instance.OnEnemyDamageDealt?.Invoke(10f);
                }
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