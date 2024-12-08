using UnityEngine;

public class PistolWeapon : MonoBehaviour, IWeapon
{
    [Header("Weapon Settings")] 
    public int damage => GameMetrics.Global.Pistol_Damage;
    private float cooldownTime => GameMetrics.Global.Pistol_CooldownTime;
    private float actualCooldown = 0;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private float bulletSpeed => GameMetrics.Global.Pistol_BulletSpeed;
    private Vector3 bulletScale => GameMetrics.Global.Pistol_BulletScale;
    
    private void Update()
    {
        actualCooldown -= Time.deltaTime;
    }

    public void AutoShoot()
    {
        if (actualCooldown > 0)
            return;
        
        actualCooldown = cooldownTime;
        
        if (bulletPrefab == null || shootPoint == null || !GameMetrics.Global.Pistol_Enabled) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.transform.localScale = bulletScale;
        bullet.GetComponent<Bullet>().Initialize(5, damage);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootPoint.forward * bulletSpeed;
        }
        
        StatsManager.Instance.OnWeaponShot?.Invoke(1);
    }
}