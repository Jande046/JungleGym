using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;

    public int[] enemiesPerWave; // Total enemies per wave (set in Inspector)
    private int currentWave = 0;
    private int enemiesSpawned = 0;
    private int enemiesKilled = 0;

    [Header("UI Settings")]
    public TextMeshProUGUI waveCounterText;
    public TextMeshProUGUI enemiesKilledText;

    [Header("Wave Timing")]
    public float timeBetweenWaves = 5f;

    private bool isSpawning = false;

    private void Start()
    {
        UpdateWaveUI();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        if (currentWave >= enemiesPerWave.Length)
        {
            Debug.Log("All waves completed!");
            yield break;
        }

        enemiesSpawned = 0;
        enemiesKilled = 0;

        int totalEnemiesInWave = enemiesPerWave[currentWave];
        UpdateEnemyKilledUI();

        isSpawning = true;

        // Spawn all enemies for the wave
        while (enemiesSpawned < totalEnemiesInWave)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(0.5f); // Delay between spawns
        }

        isSpawning = false;

        // Wait for all enemies to be killed before proceeding to the next wave
        while (enemiesKilled < totalEnemiesInWave)
        {
            yield return null;
        }

        currentWave++;
        UpdateWaveUI();

        yield return new WaitForSeconds(timeBetweenWaves);

        // Spawn the next wave
        StartCoroutine(SpawnWave());
    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyPrefab;

        // For the final wave, spawn a boss alongside enemies
        if (currentWave == enemiesPerWave.Length - 1 && Random.value < 0.1f && bossPrefab != null)
        {
            enemyPrefab = bossPrefab; // Spawn boss occasionally during the last wave
        }
        else
        {
            enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        }

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        UpdateEnemyKilledUI();

        if (enemiesKilled >= enemiesPerWave[currentWave] && !isSpawning)
        {
            Debug.Log("Wave complete!");
        }
    }

    private void UpdateWaveUI()
    {
        waveCounterText.text = $"{currentWave + 1}/{enemiesPerWave.Length}";
    }

    private void UpdateEnemyKilledUI()
    {
        enemiesKilledText.text = $"{enemiesKilled}/{enemiesPerWave[currentWave]}";
    }
}
