using System.Collections;
using System.Collections.Generic;
using LTX.Singletons;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public GameObject[] enemies;
    public SplineContainer splineContainer; // Reference to the spline
    public Transform spawnPoint;
    public float spawnDelay = 1f;
    public int enemiesPerWave = 5;
    public float movementDuration = 5f;
    private List<IEnemy> spawnedEnemies;

    void Start()
    {
        spawnedEnemies = new List<IEnemy>();
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            Debug.Log($"Enemies : {spawnedEnemies}, {spawnedEnemies.Count}");
            
            while(spawnedEnemies.Count > 0)
                yield return null;
            
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            yield return new WaitForSeconds(10f); // Delay between waves
        }
    }

    void SpawnEnemy()
    {
        if (enemies != null && splineContainer != null)
        {
            GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoint.position, Quaternion.identity);
            spawnedEnemies.Add(enemy.GetComponent<IEnemy>());
            
            if (enemy.TryGetComponent(out Enemy enemyScript))
            {
                enemyScript.Initialize(splineContainer, movementDuration);
            }
        }
    }

    public void OnEnemyKilled(IEnemy enemy)
    {
        spawnedEnemies.Remove(enemy);
    }
}