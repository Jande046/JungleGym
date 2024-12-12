using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private float expansionRate = 5f; // How quickly the sphere expands
    [SerializeField] private float maxScale = 3f; // Maximum scale of the explosion
    [SerializeField] private float duration = 1f; // How long the explosion lasts
    [SerializeField] private AudioClip explosionSound; // Explosion sound clip

    private float currentScale = 0.1f; // Starting scale
    private AudioSource audioSource;

    private void Start()
    {
        // Play the explosion sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = explosionSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Makes the sound 3D
        audioSource.Play();

        // Destroy the explosion object after its duration
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        // Gradually expand the sphere
        if (currentScale < maxScale)
        {
            currentScale += expansionRate * Time.deltaTime;
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }
}
