using System;
using Player;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject health;

    private void Start()
    {
        for (int i = 0; i < GameMetrics.Global.Health; i++)
        {
            Instantiate(health, transform);
        }

        StatsManager.Instance.OnPlayerDamageDealt += ReduceLife;
    }

    private void ReduceLife(float d)
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount-1).gameObject);
        }
    }
}
