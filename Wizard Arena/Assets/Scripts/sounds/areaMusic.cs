using UnityEngine;

public class areaMusic : MonoBehaviour
{
    public AudioSource music;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            music.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            music.Stop();
        }
    }
}
