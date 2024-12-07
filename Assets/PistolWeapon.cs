using UnityEngine;

public class PistolWeapon : MonoBehaviour, IWeapon
{
    [Header("Weapon Settings")]
    public int damage { get; } = 10;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    
    [Header("Bullet Settings")]
    public float bulletSpeed = 20f;
    public Vector3 bulletScale = new Vector3(1f, 1f, 1f);

    public void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.transform.localScale = bulletScale;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootPoint.forward * bulletSpeed;
        }
    }

    public void AutoShoot()
    {
    }
}