using UnityEngine;

public class drowning : MonoBehaviour
{
    public AudioSource Waterdeath;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Waterdeath.Play();
        }
    }
}
