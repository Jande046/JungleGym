using System.Collections;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public Transform[] spawnPoints; // Enemy spawn points
    public GameObject[] enemyPrefabs; // Array of regular enemy prefabs
    public GameObject bossPrefab; // Boss prefab
    public int[] enemiesPerWave; // Array defining the total enemies per wave
    public float spawnInterval = 1f; // Time between each spawn
    public float waveDelay = 5f; // Delay before starting the next wave

    [Header("UI Elements")]
    public TextMeshProUGUI waveText; // TextMeshPro element for wave tracking
    public TextMeshProUGUI enemiesText; // TextMeshPro element for enemy tracking

    private int currentWave = 0; // Current wave number
    private int enemiesSpawned = 0; // Total enemies spawned in the current wave
    private int enemiesKilled = 0; // Total enemies killed in the current wave
    private bool spawningWave = false; // Is the wave currently spawning enemies?

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        // Check if all enemies are killed and start the next wave after delay
        if (!spawningWave && enemiesKilled >= enemiesPerWave[currentWave] && currentWave < enemiesPerWave.Length - 1)
        {
            StartCoroutine(NextWaveCountdown());
        }
    }

    private void StartWave()
    {
        enemiesSpawned = 0;
        enemiesKilled = 0;

        UpdateUI();
        spawningWave = true;

        if (currentWave < enemiesPerWave.Length - 1)
        {
            // Spawn regular wave
            StartCoroutine(SpawnEnemies());
        }
        else
        {
            // Spawn the boss wave
            StartCoroutine(SpawnBossWave());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < enemiesPerWave[currentWave])
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
        spawningWave = false;
    }

    private IEnumerator SpawnBossWave()
    {
        // Spawn regular enemies first
        for (int i = 0; i < enemiesPerWave[currentWave] - 1; i++)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }

        // Spawn the boss enemy
        SpawnBoss();
        enemiesSpawned++;
        spawningWave = false;
    }

    private void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
    }

    private void SpawnBoss()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(bossPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
    }

    private IEnumerator NextWaveCountdown()
    {
        yield return new WaitForSeconds(waveDelay);
        currentWave++;
        StartWave();
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        waveText.text = $"Wave {currentWave + 1}/{enemiesPerWave.Length}";
        enemiesText.text = $"Enemies {enemiesKilled}/{enemiesPerWave[currentWave]}";
    }
}
