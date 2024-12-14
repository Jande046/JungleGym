using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Health : MonoBehaviour
{
    public delegate void EnemyDeathDelegate();
    public event EnemyDeathDelegate OnEnemyDeath;

    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

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
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} has died.");
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }
}
