using UnityEngine;

public class arenaBounds : MonoBehaviour
{

    public GameObject[] bounds;
    public GameObject boss;
    private bool triggered = false;

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

            // Unleash Boss
            if (boss.GetComponent<wizardBoss>())
                boss.GetComponent<wizardBoss>().Activate();
                
            else if (boss.GetComponent<statueBoss>())
                boss.GetComponent<statueBoss>().Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && triggered)
        {
            // Enable self
            triggered = false;

            // Deactivate invisible walls
            foreach (GameObject bound in bounds)
            {
                bound.GetComponent<BoxCollider2D>().enabled = false;
                Debug.Log(bound.name + " disabled");
            }

            // Silence Boss
            if (boss.GetComponent<wizardBoss>())
                boss.GetComponent<wizardBoss>().Deactivate();

            else if (boss.GetComponent<statueBoss>())
                boss.GetComponent<statueBoss>().Deactivate();
        }
    }
}
