using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRadius = 10f; // How far the enemy can detect the player
    public float attackRadius = 2f; // The radius within which the enemy will attack
    public float attackCooldown = 2f; // Time between attacks
    public float attackDamage = 20f; // Damage dealt during the attack
    public float movementSpeed = 3.5f; // Speed of the enemy

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private bool isAttacking;
    private float lastAttackTime;

    private void Awake()
    {
        // Get references to the NavMeshAgent and Player (find the player object by tag)
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Make sure the player has the tag "Player"
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // If the player is within the detection radius, pursue
            if (distanceToPlayer <= detectionRadius)
            {
                PursuePlayer();

                // If the player is within attack range and not attacking, perform attack
                if (distanceToPlayer <= attackRadius && Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                }
            }
            else
            {
                // Idle or patrol behavior (optional, can be added if desired)
                navMeshAgent.SetDestination(transform.position); // Stop moving when out of range
            }
        }
    }

    private void PursuePlayer()
    {
        if (!isAttacking)
        {
            // Set the destination of the NavMeshAgent to the player's position
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.speed = movementSpeed;
        }
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            // Trigger attack animation or effect here (e.g., deal damage)
            isAttacking = true;
            lastAttackTime = Time.time;

            // Call the player's TakeDamage method (assuming PlayerHealth script is attached to the player)
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            // You can add an attack animation trigger here if you have one

            // Attack cooldown to prevent spamming
            StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
