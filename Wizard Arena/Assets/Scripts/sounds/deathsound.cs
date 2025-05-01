using UnityEngine;

public class deathsound : MonoBehaviour
{
<<<<<<< Updated upstream
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
        if (this.CompareTag("Enemy") && enemyDestroyedSound != null)
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

=======

    public AudioClip death;
    private AudioSource audioSource;
    private bool isBeingDestroyed = false;

    void Start()
    {
        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = death;
    }

    public void DestroyObject()
    {
        if (!isBeingDestroyed)
        {
            isBeingDestroyed = true;
            StartCoroutine(PlaySoundAndDestroy());
        }
    }

    private System.Collections.IEnumerator PlaySoundAndDestroy()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}
>>>>>>> Stashed changes
