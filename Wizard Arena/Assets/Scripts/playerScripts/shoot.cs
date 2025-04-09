using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject iceshardPrefab;
    public GameObject meleePrefab;

    public float cooldownTime = 0.5f;  // Cooldown time in seconds
    private float lastAttackTime = 0f;  // Time when the last melee attack was performed

    void Start()
    {
        // Log initial mana and collision status at the start
        HealthAndMana statScript = GetComponent<HealthAndMana>();
        if (statScript != null)
        {
            Debug.Log("Starting Mana: " + statScript.currentMana + "/" + statScript.maxMana);
        }
    }

    void Update()
    {
        // Handling Ice Shard Attack (Fire3)
        if (Input.GetButtonDown("Fire3"))
        {
            HealthAndMana statScript = GetComponent<HealthAndMana>();
            if (statScript != null)
            {
                if (statScript.currentMana > 1)
                {
                    Debug.Log("Ice Shard Attack: Mana before: " + statScript.currentMana);
                    statScript.currentMana -= 1;
                    Debug.Log("Ice Shard Attack: Mana after: " + statScript.currentMana);
                    var iceShard = Instantiate(iceshardPrefab, transform.position, transform.rotation);  // Instantiate the iceshard
                }
                else
                {
                    Debug.Log("Not enough mana for Ice Shard");
                }
            }
            else
            {
                var iceShard = Instantiate(iceshardPrefab, transform.position, transform.rotation);  // Instantiate the iceshard
            }
        }

        // Handling Fireball Attack (Fire2)
        if (Input.GetButtonDown("Fire2"))
        {
            HealthAndMana statScript = GetComponent<HealthAndMana>();
            if (statScript != null)
            {
                if (statScript.currentMana > 3)
                {
                    Debug.Log("Fireball Attack: Mana before: " + statScript.currentMana);
                    statScript.currentMana -= 3;
                    Debug.Log("Fireball Attack: Mana after: " + statScript.currentMana);
                    var fireBall = Instantiate(fireballPrefab, transform.position, transform.rotation);  // Instantiate the fireball
                }
                else
                {
                    Debug.Log("Not enough mana for Fireball");
                }
            }
            else
            {
                var fireBall = Instantiate(fireballPrefab, transform.position, transform.rotation);  // Instantiate the fireball
            }
        }

        // Handling Melee Attack (Fire1)
        if (Input.GetButtonDown("Fire1"))
        {
            // Check if the cooldown has passed
            if (Time.time - lastAttackTime >= cooldownTime)
            {
                // Perform melee attack
                HealthAndMana statScript = GetComponent<HealthAndMana>();
                Debug.Log("Melee Attack Button Pressed");
                var melee = Instantiate(meleePrefab, transform.position, transform.rotation);  // Instantiate the melee

                // Add 1 mana for melee attack
                if (statScript != null)
                {
                    statScript.currentMana += 1;
                    Debug.Log("Melee Attack: Mana after: " + statScript.currentMana);
                }

                // Update the time of last attack
                lastAttackTime = Time.time;
            }
            else
            {
                Debug.Log("Melee attack is on cooldown.");
            }
        }
    }
}
