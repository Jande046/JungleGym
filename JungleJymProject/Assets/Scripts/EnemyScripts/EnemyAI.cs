using System.Collections;
using UnityEngine;
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

    private Animator animator; // Animator to control animations

    private void Awake()
    {
        // Get references to the NavMeshAgent and Player (find the player object by tag)
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Make sure the player has the tag "Player"
        animator = GetComponent<Animator>(); // Get the Animator component
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
                // Idle state when the player is out of range
                Idle();
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

            // Set animator parameters
            animator.SetFloat("speed", movementSpeed); // Set speed parameter based on movement
            animator.SetBool("attacking", false); // Ensure attack animation is off
        }
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            // Trigger attack behavior
            isAttacking = true;
            lastAttackTime = Time.time;

            // Stop moving while attacking
            navMeshAgent.SetDestination(transform.position);

            // Trigger attack animation
            animator.SetFloat("speed", 0); // Stop movement animation
            animator.SetBool("attacking", true); // Trigger attack state

            // Call the player's TakeDamage method (assuming PlayerHealth script is attached to the player)
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            // Start cooldown timer
            StartCoroutine(ResetAttack());
        }
    }

    private void Idle()
    {
        // Stop movement and set animations to idle
        navMeshAgent.SetDestination(transform.position); // Stop moving
        animator.SetFloat("speed", 0); // Ensure walking animation is off
        animator.SetBool("attacking", false); // Ensure attack animation is off
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;

        // Return to walking state if still pursuing the player
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            animator.SetBool("attacking", false);
        }
    }
}
