using UnityEngine;

public class melee : MonoBehaviour
{
    public int damage = 1;
    private float destroyDelay = 0.25f;
    private float timeSinceCreation;
    private Collider2D playerCollider;
    private Vector2 attackDirection;
    public float attackRange = 1.5f;
    public float manaCooldownTime = 1f;
    private float lastAttackTime = 0f;
    private HealthAndMana playerHealthAndMana;

    private bool active = true;

    void Start()
    {
        playerHealthAndMana = Object.FindFirstObjectByType<HealthAndMana>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

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
        Debug.Log("Melee trigger hit: " + other.gameObject.name);

        // Hit Enemy
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Melee hit an enemy!");

            if (playerHealthAndMana != null && playerHealthAndMana.currentMana < playerHealthAndMana.maxMana)
            {
                if (Time.time - lastAttackTime >= manaCooldownTime)
                {
                    playerHealthAndMana.currentMana += 1;
                    Debug.Log("Melee hit an enemy. Mana pulsed! Current Mana: " + playerHealthAndMana.currentMana);
                    lastAttackTime = Time.time;
                }
                else
                {
                    Debug.Log("Mana heal is on cooldown.");
                }
            }

            var healthComponent = other.GetComponent<enemyHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        // Hit Grave
        else if (other.CompareTag("Grave"))
        {
            Debug.Log("Melee hit a grave!");

            Grave grave = other.GetComponent<Grave>();
            if (grave != null)
            {
                grave.OnMeleeHit();
                GraveManager.Instance.ClearGrave();
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            Debug.Log("Hit non-enemy solid object: " + collision.gameObject.name);
            GetComponent<PolygonCollider2D>().enabled = false;
        }

        if (collision.gameObject.CompareTag("Enemy") && active)
        {
            Debug.Log("Hit enemy (non-trigger): " + collision.gameObject.name);
            var healthComponent = collision.gameObject.GetComponent<enemyHealth>();

            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
                active = false;

                if (playerHealthAndMana.currentMana < playerHealthAndMana.maxMana)
                {
                    playerHealthAndMana.currentMana++;
                }
            }

            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}
