using UnityEngine;

public class deathsound : MonoBehaviour
{
    public AudioClip enemyDestroyedSound;  // The sound to play when an enemy is destroyed
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // If no AudioSource attached, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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

