using UnityEngine;

public class Laser : MonoBehaviour, IWeapon
{
    public int damage => GameMetrics.Global.Laser_Damage;
    private float cooldownTime => GameMetrics.Global.Laser_CooldownTime;
    private float actualCooldown = 0;
    public GameObject laserPrefab;
    private float lifetime => GameMetrics.Global.Laser_Lifetime;
    private GameObject laser;

    private void Update()
    {
        actualCooldown -= Time.deltaTime;
    }

    public void AutoShoot()
    {
        if (actualCooldown > 0 || !GameMetrics.Global.Laser_Enabled)
            return;
        
        actualCooldown = cooldownTime;
        
        if (laser == null)
        {
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.transform.parent = transform;
            laser.GetComponent<Bullet>().lifetime = lifetime;
            
            StatsManager.Instance.OnWeaponShot?.Invoke(1);
        }
    }
}