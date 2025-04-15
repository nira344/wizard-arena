using UnityEngine;

public class arenaBounds : MonoBehaviour
{

    public GameObject[] bounds;
    private bool triggered;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject bound in bounds)
        {
            bound.GetComponent<BoxCollider2D>().enabled = false;
            Debug.Log(bound.name + " disabled");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            triggered = true;
            foreach (GameObject bound in bounds)
            {
                bound.GetComponent<BoxCollider2D>().enabled = true;
                Debug.Log(bound.name + " enabled");
            }
        }
    }
}
