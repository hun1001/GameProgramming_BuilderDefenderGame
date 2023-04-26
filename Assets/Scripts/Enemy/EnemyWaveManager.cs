using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{

    public static EnemyWaveManager Instance { get; private set; }

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
        SpawnDelay
    }

    private State state;
    private int waveNumber;

    public event EventHandler OnWaveNumberChanged;

    private float nextWaveSpawnTimer;

    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private EnemySpawner enemySpawner;

    [SerializeField] private List<EnemySpawner> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;

    //private Queue<EnemySpawner> spawnPositionTransformQueue = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        state = State.WaitingToSpawnNextWave;
        //AddRandomEnemySpawnerToQueue();
        enemySpawner = RandomEnemySpawner;
        nextWaveSpawnPositionTransform.position = enemySpawner.transform.position;
        nextWaveSpawnTimer = 3f;
    }

    // private void AddRandomEnemySpawnerToQueue() => spawnPositionTransformQueue.Enqueue(RandomEnemySpawner);
    // private Vector3 GetQueueSpawnPosition()
    // {
    //     AddRandomEnemySpawnerToQueue();
    //     return spawnPositionTransformQueue.Dequeue().transform.position;
    // }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f)
                {
                    nextWaveSpawnTimer = 10f;
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, .2f);
                        // enemySpawner.Spawning();
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            enemySpawner = RandomEnemySpawner;
                            nextWaveSpawnPositionTransform.position = enemySpawner.transform.position;
                            nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;
        }

    }

    private void SpawnWave()
    {
        state = State.SpawningWave;
        enemySpawner = RandomEnemySpawner;
        nextWaveSpawnPositionTransform.position = enemySpawner.transform.position;
        nextWaveSpawnTimer = 10f;
        remainingEnemySpawnAmount = 5 + 3 * waveNumber;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition()
    {
        return enemySpawner.transform.position;
    }

    public EnemySpawner RandomEnemySpawner => spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)];
}
