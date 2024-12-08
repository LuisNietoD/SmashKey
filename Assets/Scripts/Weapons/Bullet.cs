using Player;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private int damage;
    
    public void Initialize(float lifeTime, int damage)
    {
        this.damage = damage;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().Hit(damage);
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IEnemy>().Hit(damage);
        }
        
        Destroy(gameObject);
    }
}