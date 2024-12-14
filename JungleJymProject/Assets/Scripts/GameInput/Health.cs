using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void EnemyDeath();
    public event EnemyDeath OnEnemyDeath;

    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
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
        OnEnemyDeath?.Invoke(); // Notify listeners (e.g., WaveManager)
        Destroy(gameObject);
    }
}
