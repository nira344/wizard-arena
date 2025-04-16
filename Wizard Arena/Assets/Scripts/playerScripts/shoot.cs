using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject iceshardPrefab;
    public GameObject meleePrefab;

    public float fireballCooldownTime = 1.0f;
    public float iceshardCooldownTime = 0.5f;
    public float meleeCooldownTime = 0.1f;  // Increased for clearer cooldown


    private float lastTime = 0f;


    void Start()
    {
        HealthAndMana statScript = GetComponent<HealthAndMana>();
        if (statScript != null)
        {
            Debug.Log("Starting Mana: " + statScript.currentMana + "/" + statScript.maxMana);
        }
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            // Ice Shard Attack (Fire2)
            if (Input.GetButtonDown("Fire2"))
            {
                if (Time.time - lastTime >= iceshardCooldownTime)
                {
                    HealthAndMana statScript = GetComponent<HealthAndMana>();
                    if (statScript != null)
                    {
                        if (statScript.currentMana + statScript.currentHealth > 1)
                        {
                            statScript.currentMana -= 1;
                            Instantiate(iceshardPrefab, transform.position, transform.rotation);
                        }
                        else
                        {
                            Debug.Log("Not enough mana for Ice Shard");
                        }
                    }

                    lastTime = Time.time;
                }
                else
                {
                    Debug.Log("Ice shard on cooldown.");
                }
            }

            // Fireball Attack (Fire3)
            if (Input.GetButtonDown("Fire3"))
            {
                if (Time.time - lastTime >= fireballCooldownTime)
                {
                    HealthAndMana statScript = GetComponent<HealthAndMana>();
                    if (statScript != null)
                    {
                        if (statScript.currentMana + statScript.currentHealth > 3)
                        {
                            statScript.currentMana -= 3;
                            Instantiate(fireballPrefab, transform.position, transform.rotation);
                        }
                        else
                        {
                            Debug.Log("Not enough mana for Fireball");
                        }
                    }

                    lastTime = Time.time;
                }
                else
                {
                    Debug.Log("Fireball on cooldown.");
                }
            }

            // Melee Attack (Fire1)
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Melee Attack Button Pressed");
                Instantiate(meleePrefab, transform.position, transform.rotation);

                HealthAndMana statScript = GetComponent<HealthAndMana>();
                if (statScript != null)
                {
                    Debug.Log("Melee Attack: Mana after: " + statScript.currentMana);
                }
            }
        }
    }
}