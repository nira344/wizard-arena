using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject iceshardPrefab;
    public GameObject meleePrefab;

    public float fireballCooldownTime = 0.5f;
    public float iceshardCooldownTime = 0.2f;
    public float meleeCooldownTime = 5f;  // Increased for clearer cooldown

    private float lastFireballTime = 0f;
    private float lastIceShardTime = 0f;
    private float lastMeleeTime = 0f;

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
        // Ice Shard Attack (Fire2)
        if (Input.GetButtonDown("Fire2"))
        {
            if (Time.time - lastIceShardTime >= iceshardCooldownTime)
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

                lastIceShardTime = Time.time;
            }
            else
            {
                Debug.Log("Ice shard on cooldown.");
            }
        }

        // Fireball Attack (Fire3)
        if (Input.GetButtonDown("Fire3"))
        {
            if (Time.time - lastFireballTime >= fireballCooldownTime)
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

                lastFireballTime = Time.time;
            }
            else
            {
                Debug.Log("Fireball on cooldown.");
            }
        }

        // Melee Attack (Fire1)
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time - lastMeleeTime >= meleeCooldownTime)
            {
                Instantiate(meleePrefab, transform.position, transform.rotation);
                lastMeleeTime = Time.time;
            }
            else
            {
                Debug.Log("Melee on cooldown.");
            }
        }
    }
}
