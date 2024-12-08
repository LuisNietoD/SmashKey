using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionPrefab;
    private float explosionRadius;
    private float yExplosion;
    public int damage;

    public void Initialize(float delay, float radius, int damage)
    {
        yExplosion = delay;
        explosionRadius = radius;
        this.damage = damage;
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
            if (hit.CompareTag("Enemy") && hit.TryGetComponent(out IEnemy enemy))
            {
                enemy.Hit(damage);
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