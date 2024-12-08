using UnityEngine;

public class PistolWeapon : MonoBehaviour, IWeapon
{
    [Header("Weapon Settings")] public int damage { get; } = 10;

    public float cooldownTime = 1.2f;
    private float actualCooldown = 0;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    [Header("Bullet Settings")] public float bulletSpeed = 20f;
    public Vector3 bulletScale = new Vector3(1f, 1f, 1f);
    
    private void Update()
    {
        actualCooldown -= Time.deltaTime;
    }

    public void AutoShoot()
    {
        if (actualCooldown > 0)
            return;
        
        actualCooldown = cooldownTime;
        
        if (bulletPrefab == null || shootPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.transform.localScale = bulletScale;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootPoint.forward * bulletSpeed;
        }
        
        StatsManager.Instance.OnWeaponShot?.Invoke(1);
    }
}