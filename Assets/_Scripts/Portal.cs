using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject[][] Waves;

    private float nextSpawn = 0;

    void Update()
    {
        if(Time.time > nextSpawn)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = transform.position;

        nextSpawn = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
    }
}
