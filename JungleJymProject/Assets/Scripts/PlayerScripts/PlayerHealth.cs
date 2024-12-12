using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;

    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f; // how long it takes the health bar to catch up
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    [Header("Respawn Settings")]
    public Transform spawnPoint; // Set the spawn point in the inspector
    public float respawnDelay = 3f; // Delay before respawning

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        UpdateHealthUI(); // Make sure the UI is updated at the start
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);  
        UpdateHealthUI(); // Update UI whenever health changes
    }

    public void UpdateHealthUI()
    {
        // Update health text
        if (healthText != null)
        {
            healthText.text = "Health: " + health.ToString("F0");
        }

        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        // Update the health bar smoothly
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    // This method is called when the player receives damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        if (health <= 0)
        {
            Die();
        }
    }

    // This method handles the player's death
    public void Die()
    {
        Debug.Log("Player has died");

        // Disable the player object temporarily
        gameObject.SetActive(false);

        // Call respawn after a delay using Invoke
        Invoke("Respawn", respawnDelay); // We use Invoke instead of StartCoroutine here
    }

    // Respawn the player at the spawn point after a delay
    private void Respawn()
    {
        // Set the player's position to the spawn point and enable the player object again
        transform.position = spawnPoint.position;
        gameObject.SetActive(true);

        // Reset the health
        health = maxHealth;

        // Update the health UI
        UpdateHealthUI();
    }

    // This method is called when the player takes damage from an enemy
    public void DamageFromEnemy(float damage)
    {
        TakeDamage(damage);
    }
}
