using System.Collections;
using System.Collections.Generic;
using LTX.Singletons;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public GameObject[] enemies;
    public SplineContainer splineContainer; // Reference to the spline
    public float spawnDelay = 1f;
    public int enemiesPerWave = 5;
    public float movementDuration = 10f;
    private List<IEnemy> spawnedEnemies;

    [SerializeField] private Collider spawnBound;

    private float fireRate;
    private int health;

    private float gameStartTime;
    private int additionalEnemiesPerMinute = 1;

    private void Start()
    {
        spawnedEnemies = new List<IEnemy>();
        gameStartTime = Time.time;
        
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (true)
        {
            int currentEnemiesPerWave = CalculateEnemiesForWave();
            
            for (int i = 0; i < currentEnemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            yield return new WaitForSeconds(10f); // Delay between waves
        }
    }

    private int CalculateEnemiesForWave()
    {
        float elapsedMinutes = (Time.time - gameStartTime) / 10f;
        int additionalEnemies = Mathf.FloorToInt(elapsedMinutes) * additionalEnemiesPerMinute;
        return enemiesPerWave + additionalEnemies;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 min = spawnBound.bounds.min;
        Vector3 max = spawnBound.bounds.max;
        
        float x = Random.Range(min.x, max.x);
        float z = Random.Range(min.z, max.z);
        
        return new Vector3(x, max.y, z);
    }

    private void GetAttributes()
    {
        float elapsedSec = (Time.time - gameStartTime) / 30f;
        
        health += Mathf.FloorToInt(elapsedSec * 5);
        fireRate = Mathf.Min(0.9f, elapsedSec * 0.1f);
        
        Debug.Log($"Health : {health} | FireRate : {fireRate}");
    }

    private void SpawnEnemy()
    {
        if (enemies != null && splineContainer != null)
        {
            GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], GetSpawnPosition(), transform.rotation);
            spawnedEnemies.Add(enemy.GetComponent<IEnemy>());
            
            if (enemy.TryGetComponent(out Enemy enemyScript))
            {
                GetAttributes();
                enemyScript.Initialize(splineContainer, movementDuration, health, fireRate);
            }

            StatsManager.Instance.OnEnemySpawned(1);
        }
    }

    public void OnEnemyKilled(IEnemy enemy)
    {
        spawnedEnemies.Remove(enemy);
    }
}