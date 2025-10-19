using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private WaveData[] waves;
    private int _currentWaveIndex = 0;
    private WaveData CurrentWave => waves[_currentWaveIndex];
    private float _spawnTimer;
    private float _spawnCounter;
    private float _enemiesRemoved;

    [Header("Object Pools")]

    [SerializeField] private ObjectPooler copasPool;
    [SerializeField] private ObjectPooler espadasPool;
    [SerializeField] private ObjectPooler ourosPool;
    [SerializeField] private ObjectPooler pausPool;

    private Dictionary<EnemyType, ObjectPooler> _poolDictionary;
    
    private void Awake()
    {
        _poolDictionary = new Dictionary<EnemyType, ObjectPooler>()
        {
            { EnemyType.Copas, copasPool },
            { EnemyType.Espadas, espadasPool },
            { EnemyType.Ouros, ourosPool },
            { EnemyType.Paus, pausPool }
        };
    }

    void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if( _spawnTimer <= 0 && _spawnCounter < CurrentWave.enemiesPerWave)
        {
            _spawnTimer = CurrentWave.spawnInterval;
            SpawnEnemy();
            _spawnCounter++;
        } else if(_spawnCounter >= CurrentWave.enemiesPerWave && _enemiesRemoved >= CurrentWave.enemiesPerWave)
        {
            _currentWaveIndex = (_currentWaveIndex + 1) % waves.Length;
            _spawnCounter = 0;
        }
    }

    private void SpawnEnemy()
    {
        if(_poolDictionary.TryGetValue(CurrentWave.enemyType, out var pool))
        {
            GameObject spawnedObject = pool.GetPooledObject();
            spawnedObject.transform.position = transform.position;
            spawnedObject.SetActive(true);
        }
        
    }
}
