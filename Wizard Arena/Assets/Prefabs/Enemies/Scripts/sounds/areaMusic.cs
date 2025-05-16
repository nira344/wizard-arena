using UnityEngine;

public class areaMusic : MonoBehaviour
{
    public AudioSource music;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            music.Play();
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
