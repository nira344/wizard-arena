using UnityEngine;

public class areaMusic : MonoBehaviour
{
    public AudioSource music;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("something just happened");
        if (collision.CompareTag("Player"))
        {
            Debug.LogError("playing music");
            music.Play();
            Debug.LogError("music started");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            music.Stop();
        }
    }
}
