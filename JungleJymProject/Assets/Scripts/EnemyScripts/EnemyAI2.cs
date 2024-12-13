using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI2 : MonoBehaviour
{
    public Transform target; // Player or target to pursue
    private EnemyReferences enemyReferences;

    private float pathUpdateDeadline;
    private float attackDistance;

    [Header("Attack Settings")]
    public float attackCooldown = 1.5f; // Time between attacks
    public float attackDamage = 20f; // Damage dealt to the player

    private bool isAttacking;

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
    }

    void Start()
    {
        // Dynamically assign the player as the target
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("Player object not found in the scene! Ensure your player has the 'Player' tag.");
            }
        }
        if (enemyReferences != null && enemyReferences.navMeshAgent != null)
        {
            attackDistance = enemyReferences.navMeshAgent.stoppingDistance;
        }
        else
        {
            Debug.LogError("NavMeshAgent is not assigned in EnemyReferences.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            bool inRange = distanceToTarget <= attackDistance;

            if (inRange)
            {
                LookAtTarget();

                // Trigger attack if not already attacking
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
            else
            {
                UpdatePath();
            }

            // Update animator parameters
            enemyReferences.animator.SetBool("Attacking", inRange && isAttacking);
            enemyReferences.animator.SetFloat("Speed", inRange ? 0 : enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude);
        }
    }

    private void LookAtTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void UpdatePath()
    {
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + enemyReferences.pathUpdateDelay;
            enemyReferences.navMeshAgent.SetDestination(target.position);
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        // Trigger attack animation
        enemyReferences.animator.SetBool("Attacking", true);

        // Deal damage to player
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        // Wait for the attack cooldown
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}
