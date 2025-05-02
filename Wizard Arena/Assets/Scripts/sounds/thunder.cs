using UnityEngine;

public class tunder : MonoBehaviour
{
    
    public AudioSource audiosource;
    public AudioClip idle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
          if (audiosource == null)
        {
            audiosource = GetComponent<AudioSource>();
        }

        // Start the Coroutine to play the idle sound at random intervals
        if (audiosource != null && idle != null)
        {
            StartCoroutine(PlayIdleSound());
        }
        else
        {
            Debug.LogWarning("AudioSource or IdleSoundClip is missing.");
        }
    }

    // Coroutine to play the idle sound at random intervals
    private System.Collections.IEnumerator PlayIdleSound()
    {
        while (true)
        {
            // Wait for a random time between 7 and 10 seconds
            float waitTime = Random.Range(10f, 30f);
            yield return new WaitForSeconds(waitTime);

            // Play the idle sound
            audiosource.PlayOneShot(idle);
        }
    }
}
