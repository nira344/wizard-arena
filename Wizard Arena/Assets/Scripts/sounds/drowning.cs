using UnityEngine;

public class drowning : MonoBehaviour
{
    public AudioSource waterdeath;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            waterdeath.Play();
        }
    }
}
