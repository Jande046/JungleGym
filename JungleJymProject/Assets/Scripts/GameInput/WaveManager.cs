using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;

    public int[] enemiesPerWave;

    private int currentWave = 0;
    private int enemiesSpawned = 0;
    private int enemiesKilled = 0;

    [Header("UI Settings")]
    public TextMeshProUGUI waveCounterText;
    public TextMeshProUGUI enemiesKilledText;

    [Header("Wave Timing")]
    public float timeBetweenWaves = 5f;
    private bool waveInProgress = false;

    private void Start()
    {
        UpdateWaveUI();
        StartCoroutine(SpawnWave());
    }

    private void Update()
    {
        if (waveInProgress && enemiesKilled == enemiesPerWave[currentWave])
        {
            waveInProgress = false;
            StartCoroutine(StartNextWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        waveInProgress = true;
        enemiesSpawned = 0;
        enemiesKilled = 0;

        UpdateEnemyKilledUI();

        
        while (enemiesSpawned < enemiesPerWave[currentWave])
        {
            SpawnEnemy();
            enemiesSpawned++;
            Debug.Log($"Enemy spawned: {enemiesSpawned}/{enemiesPerWave[currentWave]}");
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log($"Wave {currentWave + 1}: All enemies spawned.");
    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyPrefab = (currentWave == enemiesPerWave.Length - 1 && bossPrefab != null && enemiesSpawned == 0)
            ? bossPrefab
            : enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDeath += HandleEnemyDeath;
        }
    }

    public void HandleEnemyDeath()
{
    enemiesKilled++;
    Debug.Log($"Enemy killed event triggered. Current count: {enemiesKilled}/{enemiesPerWave[currentWave]}");

    UpdateEnemyKilledUI();

    // Check for wave completion
    if (waveInProgress && enemiesKilled == enemiesPerWave[currentWave])
    {
        waveInProgress = false;
        if (currentWave + 1 >= enemiesPerWave.Length)
        {
            Debug.Log("All waves completed!");
            EndGame();
        }
        else
        {
            StartCoroutine(StartNextWave());
        }
    }
}

    private IEnumerator StartNextWave()
    {
        Debug.Log("Starting next wave...");
        yield return new WaitForSeconds(timeBetweenWaves);

        currentWave++;
        if (currentWave < enemiesPerWave.Length)
        {
            UpdateWaveUI();
            StartCoroutine(SpawnWave());
        }
        else
        {
            Debug.Log("All waves completed!");
            EndGame();
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

    private void EndGame()
    {
        Debug.Log("Transitioning to Game Over scene...");
        SceneManager.LoadScene("GameOver");
    }
}
