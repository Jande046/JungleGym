using System.Collections;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;

    public int[] enemiesPerWave; // Number of enemies for each wave

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
        // Check if all enemies for the wave are dead
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

        // Spawn enemies for the current wave
        while (enemiesSpawned < enemiesPerWave[currentWave])
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(0.5f); // Delay between spawns
        }

        Debug.Log($"Wave {currentWave + 1}: All enemies spawned.");
    }

   private void SpawnEnemy()
{
    if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0)
    {
        Debug.LogError("No spawn points or enemy prefabs assigned!");
        return;
    }

    // Choose a random spawn point and enemy prefab
    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
    GameObject enemyPrefab = (currentWave == enemiesPerWave.Length - 1 && bossPrefab != null && enemiesSpawned == 0)
        ? bossPrefab
        : enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

    // Spawn the enemy
    GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

    // Subscribe to the enemy's death event (assuming the Health script is used)
    Health enemyHealth = enemy.GetComponent<Health>();
    if (enemyHealth != null)
    {
        enemyHealth.OnEnemyDeath += HandleEnemyDeath;
    }

    Debug.Log($"Spawned enemy at {spawnPoint.position}. Current wave: {currentWave + 1}, Enemies spawned: {enemiesSpawned + 1}");
}


    public void HandleEnemyDeath()
    {
        enemiesKilled++;
        UpdateEnemyKilledUI();
    }

    private IEnumerator StartNextWave()
{
    Debug.Log("Starting next wave in a few seconds...");
    yield return new WaitForSeconds(timeBetweenWaves);

    currentWave++;

    // Check if there are more waves
    if (currentWave < enemiesPerWave.Length)
    {
        UpdateWaveUI();
        StartCoroutine(SpawnWave());
    }
    else
    {
        EndGame(); // Call EndGame when all waves are completed
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
    Debug.Log("All waves completed! Game Over.");
    waveCounterText.text = "Game Over!"; // Update the wave counter UI to indicate the game has ended
    enemiesKilledText.text = "";        // Clear the enemies killed text

    // Additional logic for ending the game (e.g., showing a menu, stopping gameplay, etc.)
    Time.timeScale = 0; // Pause the game (optional)
}



}
