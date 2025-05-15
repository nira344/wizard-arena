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
            bound.SetActive(false);
        }
    }

    void Update()
    {
        if (boss == null)
        {
            // Disable invisible walls
            foreach (GameObject bound in bounds)
            {
                bound.SetActive(false);
            }
            gameObject.SetActive(false);
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
                bound.SetActive(true);
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
                bound.SetActive(false);
            }

            // Silence Boss
            if (boss.GetComponent<wizardBoss>())
                boss.GetComponent<wizardBoss>().Deactivate();

            else if (boss.GetComponent<statueBoss>())
                boss.GetComponent<statueBoss>().Deactivate();
        }
    }
}
