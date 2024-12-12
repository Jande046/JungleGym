using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float damage = 50f; // Damage dealt by the explosion
    [SerializeField] private float explosionRadius = 5f; // Radius of the explosion
    [SerializeField] private GameObject explosionEffect; // Optional visual effect for the explosion
    [SerializeField] private LayerMask damageableLayers; // Layers to apply damage to
    [SerializeField] private float lifetime = 5f; // Time before the projectile disappears

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after a set lifetime
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode(); // Call the explosion logic
        Destroy(gameObject); // Destroy the projectile
    }

    private void Explode()
    {
        // Show explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Find all objects within the explosion radius
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius, damageableLayers);

       /* foreach (Collider hit in hitObjects)
        {
            // Apply damage if the object has a health component
            Health targetHealth = hit.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the explosion radius in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
