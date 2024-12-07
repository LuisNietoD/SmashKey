using UnityEngine;

public class Laser : MonoBehaviour, IWeapon
{
    public int damage { get; }
    public float cooldownTime = 5f;
    private float actualCooldown = 0;
    public GameObject laserPrefab;
    public float lifetime;
    private GameObject laser;

    private void Update()
    {
        actualCooldown -= Time.deltaTime;
    }

    public void AutoShoot()
    {
        if (actualCooldown > 0)
            return;
        
        actualCooldown = cooldownTime;
        
        if (laser == null)
        {
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.transform.parent = transform;
            laser.GetComponent<Bullet>().lifetime = lifetime;
            
            StatsManager.Instance.OnWeaponBulletShot?.Invoke(this, 1);
        }
    }
}