using UnityEngine;

public class arenaBounds : MonoBehaviour
{

    public GameObject[] bounds;
    public GameObject boss;
    private bool triggered;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Disable invisible walls
        foreach (GameObject bound in bounds)
        {
            bound.GetComponent<BoxCollider2D>().enabled = false;
            Debug.Log(bound.name + " disabled");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            // Disable self
            triggered = true;

            // Activate invisible walls
            foreach (GameObject bound in bounds)
            {
                bound.GetComponent<BoxCollider2D>().enabled = true;
                Debug.Log(bound.name + " enabled");
            }

            // UNLEASH GILBERT
            boss.GetComponent<wizardBoss>().Activate();
        }
    }
}
