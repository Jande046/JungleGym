using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private WaveManager waveManager;

    private void Start()
    {
        currentHealth = maxHealth;
        waveManager = FindObjectOfType<WaveManager>(); // Find the WaveManager in the scene
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        // Notify the WaveManager that an enemy has been killed
        if (waveManager != null && gameObject.CompareTag("Enemy")) // Ensure this is an enemy
        {
            waveManager.EnemyKilled();
        }

        Destroy(gameObject); // Destroy the object when health reaches 0
    }
}
