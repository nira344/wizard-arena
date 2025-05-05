using UnityEngine;

public class melee : MonoBehaviour
{
    public int damage = 1;
    private float destroyDelay = 0.25f;
    private float timeSinceCreation;
    private Collider2D playerCollider;
    private Vector2 attackDirection;
    public float attackRange = 3f;

    private static float manaCooldownTime = 0.5f;
    private static float lastManaGainTime = -999f;

    private HealthAndMana playerHealthAndMana;
    private Transform playerTransform;

    private bool active = true;

    void Start()
    {
        playerHealthAndMana = Object.FindFirstObjectByType<HealthAndMana>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();
            playerTransform = player.transform;

            // Set as child so it moves with player
            transform.SetParent(playerTransform);
        }

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
        // Always follow player
        PositionAndRotateMeleeObject();

        if (Time.time - timeSinceCreation >= destroyDelay)
        {
            Destroy(gameObject);
        }
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
            HandleGraveHit(other);
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
        }

        var healthComponent = other.GetComponent<enemyHealth>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damage);
        }

        // Optional: comment out to let it stay for full duration
        // Destroy(gameObject);
    }

    private void HandleChestHit(Collider2D other)
    {
        Chest chest = other.GetComponent<Chest>();
        if (chest != null)
        {
            chest.OnMeleeHit();
            ChestManager.Instance.ClearChest();
        }

        Destroy(gameObject);
    }

    private void HandleGraveHit(Collider2D other)
    {
        Grave grave = other.GetComponent<Grave>();
        if (grave != null)
        {
            grave.OnMeleeHit();
            GraveManager.Instance.ClearGrave();
        }

        Destroy(gameObject);
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
                }
            }

            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    private void PositionAndRotateMeleeObject()
    {
        if (playerTransform == null) return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        attackDirection = (mouseWorldPosition - playerTransform.position).normalized;

        // Position the melee hitbox farther away based on attackRange
        transform.position = (Vector2)playerTransform.position + attackDirection * attackRange;

        // Rotate to face the direction of the attack
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
}
