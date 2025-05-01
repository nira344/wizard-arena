using UnityEngine;

public class scriptforthepickupsound : MonoBehaviour
{
   public AudioSource pop;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("item")) // Make sure your player is tagged "Player"
        {
            pop.Play();
        }
    }
}
