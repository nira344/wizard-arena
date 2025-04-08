using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject iceshardPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            HealthAndMana statScript = GetComponent<HealthAndMana>();
            if (statScript != null)
            {
                if (statScript.currentMana >= 1)
                {
                    statScript.currentMana--;
                    var iceShard = Instantiate(iceshardPrefab, transform.position, transform.rotation);  // Instantiate the iceshard
                }
            }
            else
            {
                var iceShard = Instantiate(iceshardPrefab, transform.position, transform.rotation);  // Instantiate the iceshard
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            HealthAndMana statScript = GetComponent<HealthAndMana>();
            if (statScript != null)
            {
                if (statScript.currentMana >= 3)
                {
                    statScript.currentMana-= 3;
                    var fierBall = Instantiate(fireballPrefab, transform.position, transform.rotation);  // Instantiate the fireball
                }
            }
            else
            {
                var fireBall = Instantiate(fireballPrefab, transform.position, transform.rotation);  // Instantiate the fireball
            }
        }
    }
}
