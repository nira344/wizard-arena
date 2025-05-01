using UnityEngine;

public class deathwatcher : MonoBehaviour
{
     private AudioClip destroySound;

    public void Setup(AudioClip clip)
    {
        destroySound = clip;
    }

    void OnDestroy()
    {
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
    }
}
