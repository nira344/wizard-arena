using UnityEngine;

public class respawnTrigger : MonoBehaviour
{
    public GameObject[] previousRespawnPoints;
    public GameObject newRespawn;

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject respawns in previousRespawnPoints)
            {
                respawns.SetActive(false);
            }
            newRespawn.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
