using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    public List<GameObject> enemies = new List<GameObject>();
    public float spawnRate;
    
    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnRate);
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemies.Count);

        Instantiate(enemies[index]);
        StatsManager.Instance.OnEnemySpawned?.Invoke(1);
    }
}