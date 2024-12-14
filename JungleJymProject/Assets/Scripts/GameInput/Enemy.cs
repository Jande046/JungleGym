using UnityEngine;

public class Enemy : MonoBehaviour
{
    private WaveManager waveManager;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        if (waveManager == null)
        {
            Debug.LogError("WaveManager not found in the scene. Ensure it exists.");
        }
    }

    public void Die()
    {
        // Notify the WaveManager
        if (waveManager != null)
        {
            waveManager.HandleEnemyDeath(); // Call the correct method
        }

        // Destroy the enemy game object
        Destroy(gameObject);
    }
}
