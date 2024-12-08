using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public SplineContainer splineContainer; // Reference to the spline
    public float spawnDelay = 1f;
    public int enemiesPerWave = 5;
    public float movementDuration = 5f;
    public List<GameObject> enemies;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            while(enemies.Count > 0)
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
        if (enemyPrefab != null && splineContainer != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, splineContainer.transform.position, Quaternion.identity);
            enemies.Add(enemy);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Initialize(splineContainer, movementDuration);
            }
        }
    }
}