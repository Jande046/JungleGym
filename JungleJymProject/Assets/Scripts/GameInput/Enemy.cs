using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private WaveManager waveManager;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    public void Die()
    {
        // Notify the WaveManager
        waveManager.EnemyKilled();
        Destroy(gameObject);
    }
}
