using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour {

    public static EnemyWaveManager Instance { get; private set; }

    private enum State {
        WaitingToSpawnNextWave,
        SpawningWave
    }

    private State state;
    private int waveNumber;

    public event EventHandler OnWaveNumberChanged;

    private float nextWaveSpawnTimer;

    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        nextWaveSpawnTimer = 3f;
    }

    private void Update() {
        switch (state) {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f) {
                    nextWaveSpawnTimer = 10f;
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0) {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f) {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, .2f);
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0) {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;
        }

    }

    private void SpawnWave() {
        state = State.SpawningWave;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        nextWaveSpawnTimer = 10f;
        remainingEnemySpawnAmount = 5 + 3 * waveNumber;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }


    public int GetWaveNumber() {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer() {
        return nextWaveSpawnTimer;
    }
    public Vector3 GetSpawnPosition() {
        return spawnPosition;
    }

}
