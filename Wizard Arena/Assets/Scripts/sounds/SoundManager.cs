using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip soundClip;
    public AudioSource audioSource; // Assign in the Inspector
    private bool isPlaying = false;

    public void PlaySound()
    {
        if (!isPlaying)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
            isPlaying = true;
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            isPlaying = false;
        }
    }
}

