using UnityEngine;

public class melee : MonoBehaviour
{
    public int damage = 1;
    private float destroyDelay = 0.25f;
    private float timeSinceCreation;
    private Collider2D playerCollider;
    private Vector2 attackDirection;
    public float attackRange = 1.5f;

    private static float manaCooldownTime = 0.5f;
    private static float lastManaGainTime = -999f;

    private HealthAndMana playerHealthAndMana;

    private bool active = true;

    void Start()
    {
        playerHealthAndMana = Object.FindFirstObjectByType<HealthAndMana>();
        playerCollider = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider2D>();

        if (playerCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>(), true);
        }

        timeSinceCreation = Time.time;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        PositionAndRotateMeleeObject();
    }

    void Update()
    {
        if (Time.time - timeSinceCreation >= destroyDelay)
        {
            Destroy(gameObject);
        }
    }

    private void PositionAndRotateMeleeObject()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        attackDirection = (mouseWorldPosition - transform.position).normalized;
        transform.position = (Vector2)transform.position + attackDirection * attackRange;

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HandleEnemyHit(other);
        }
        else if (other.CompareTag("Chest"))
        {
            HandleChestHit(other);
        }
        else if (other.CompareTag("Grave"))
        {
            HandleGraveHit(other);  // Ensure interaction with the grave
        }
    }

    private void HandleEnemyHit(Collider2D other)
    {
        if (playerHealthAndMana != null && playerHealthAndMana.currentMana < playerHealthAndMana.maxMana)
        {
            if (Time.time - lastManaGainTime >= manaCooldownTime)
            {
                playerHealthAndMana.currentMana += 1;
                Debug.Log("Melee hit an enemy. Mana gained! Current Mana: " + playerHealthAndMana.currentMana);
                lastManaGainTime = Time.time;
            }
            else
            {
                Debug.Log("Mana gain on cooldown.");
            }
        }

        var healthComponent = other.GetComponent<enemyHealth>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damage);
        }

        Destroy(gameObject); // Destroy melee object after collision
    }

    private void HandleChestHit(Collider2D other)
    {
        Chest chest = other.GetComponent<Chest>();
        if (chest != null)
        {
            chest.OnMeleeHit(); // This calls OnMeleeHit from the Chest script
            ChestManager.Instance.ClearChest(); // Clear chest manager
        }

        Destroy(gameObject); // Destroy melee object after hitting chest
    }

    private void HandleGraveHit(Collider2D other)
    {
        Grave grave = other.GetComponent<Grave>();
        if (grave != null)
        {
            grave.OnMeleeHit(); // This calls OnMeleeHit from the Grave script
            GraveManager.Instance.ClearGrave(); // Clear grave manager
        }

        Destroy(gameObject); // Destroy melee object after hitting grave
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            Debug.Log("Hit solid object: " + collision.gameObject.name);
            GetComponent<PolygonCollider2D>().enabled = false;
        }

        if (collision.gameObject.CompareTag("Enemy") && active)
        {
            var healthComponent = collision.gameObject.GetComponent<enemyHealth>();

            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
                active = false;

                if (playerHealthAndMana != null && playerHealthAndMana.currentMana < playerHealthAndMana.maxMana)
                {
                    if (Time.time - lastManaGainTime >= manaCooldownTime)
                    {
                        playerHealthAndMana.currentMana += 1;
                        Debug.Log("Melee (collision) hit enemy. Mana gained.");
                        lastManaGainTime = Time.time;
                    }
                    else
                    {
                        Debug.Log("Mana gain on cooldown (collision).");
                    }
                }
            }

            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}
