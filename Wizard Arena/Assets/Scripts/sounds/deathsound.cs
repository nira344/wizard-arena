using UnityEngine;

public class deathsound : MonoBehaviour
{
    public AudioClip enemyDestroyedSound;  // The sound to play when an enemy is destroyed
    private AudioSource audioSource;

    void Start()
    {
        if (audioSource == null)
        {
            // If no AudioSource is attached, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Ensure that there's an AudioSource on the same GameObject
        audioSource = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
        // When the script or object is destroyed, check for any object with "Enemy" tag
        if (this.CompareTag("Enemy") || this.CompareTag("Player") && enemyDestroyedSound != null)
        {
            PlayEnemyDestroyedSound();
        }
    }

    // Method to play the sound
    public void PlayEnemyDestroyedSound()
    {
        audioSource.PlayOneShot(enemyDestroyedSound);
    }
}

