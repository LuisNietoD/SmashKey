using UnityEngine;
using System.Collections;

public class SymmetricalShooter : MonoBehaviour, IWeapon
{
    public int damage { get; } = 10;
    public float cooldownTime = 0.5f;
    private float actualCooldown = 0;
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float bulletSpeed = 10f;
    public Vector3 bulletScale = new Vector3(1f, 1f, 1f);
    public int bulletCount = 2;
    public float spawnOffset = 1f;
    public float offsetBetweenBullets = 1f;
    public float spawnDelay = 0.03f;

    private void Update()
    {
        actualCooldown -= Time.deltaTime;
    }
    
    public void AutoShoot()
    {
        if (actualCooldown > 0)
            return;
        
        actualCooldown = cooldownTime;
        
        if (bulletPrefab == null || spawnPoint == null) return;

        StartCoroutine(ShootWithDelay());
    }

    private IEnumerator ShootWithDelay()
    {
        int[] indices = GenerateRandomOrder(bulletCount);

        for (int i = 0; i < bulletCount; i++)
        {
            int index = indices[i];
            float offset = spawnOffset + index * offsetBetweenBullets;

            // Right bullet
            SpawnBullet(spawnPoint.position + spawnPoint.right * offset, spawnPoint.forward);

            // Left bullet
            SpawnBullet(spawnPoint.position - spawnPoint.right * offset, spawnPoint.forward);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnBullet(Vector3 position, Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction));
        bullet.transform.localScale = bulletScale;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }

    private int[] GenerateRandomOrder(int count)
    {
        int[] indices = new int[count];
        for (int i = 0; i < count; i++) indices[i] = i;

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, count);
            (indices[i], indices[randomIndex]) = (indices[randomIndex], indices[i]);
        }

        return indices;
    }
}