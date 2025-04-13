using UnityEngine;


public class melee : MonoBehaviour
{
   public int damage = 1;  // Configurable from the Inspector
   private float destroyDelay = 0.25f; // Time before destroying the object
   private float timeSinceCreation; // Time elapsed since the object was created
   private Collider2D playerCollider; // Reference to the player's collider
   private Vector2 attackDirection; // Direction of the melee attack
   public float attackRange = 1.5f; // Distance at which the melee attack should appear from the player
   public float manaCooldownTime = 1f;  // Cooldown time for mana heal
   private float lastAttackTime = 0f; // Last time the melee attack happened
   private HealthAndMana playerHealthAndMana;


   private bool active = true;


   void Start()
   {
       playerHealthAndMana = Object.FindFirstObjectByType<HealthAndMana>(); // Find the player HealthAndMana script


       // Find the player's collider (assuming the player is tagged "Player")
       playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();


       // Ignore collisions between the melee attack and the player
       if (playerCollider != null)
       {
           Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>(), true);
       }


       // Record the time at which the object was created
       timeSinceCreation = Time.time;


       // Rotate the melee object by 90 degrees to fix the upward-facing issue
       transform.rotation = Quaternion.Euler(0, 0, -90); // Rotate to face right (corrects upward-facing sprite)


       // Position the melee object relative to the player and rotate it towards the mouse
       PositionAndRotateMeleeObject();
   }


   void Update()
   {
       // If the time since creation is greater than or equal to the destroyDelay, destroy the object
       if (Time.time - timeSinceCreation >= destroyDelay)
       {
           Destroy(gameObject);  // Destroy the object
       }
   }


   private void PositionAndRotateMeleeObject()
   {
       // Get the mouse position in the world
       Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       mouseWorldPosition.z = 0;  // Ensure the z value stays at 0, since we're working in 2D


       // Calculate the direction from the player to the mouse
       attackDirection = (mouseWorldPosition - transform.position).normalized;


       // Place the melee attack object a specified distance away from the player in the attack direction
       transform.position = (Vector2)transform.position + attackDirection * attackRange;


       // Calculate the angle between the melee attack and the mouse position
       float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;


       // Apply the rotation to the melee attack object
       transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));  // Corrected rotation
   }


   private void OnCollisionEnter2D(Collision2D collision)
   {
       if (collision.gameObject.tag == "Solid")
       {
           Debug.Log("Hit non-enemy " + collision.gameObject);
           GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
       }


       // Check if the collision is with an object tagged "Enemy"
       if (collision.gameObject.tag.Equals("Enemy") && active)
       {
           Debug.Log("Hit enemy " + collision.gameObject);
           var healthComponent = collision.gameObject.GetComponent<enemyHealth>();


           if (healthComponent != null)
           {
               healthComponent.TakeDamage(damage);  // Use the damage variable
               active = false;
               GameObject player = GameObject.FindGameObjectWithTag("Player");
               HealthAndMana manaComponent = player.GetComponent<HealthAndMana>();
               if (manaComponent.currentMana < manaComponent.maxMana)
               {
                   manaComponent.currentMana++;
               }
           }


           GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
       }
   }


   // This will be called when the melee object collides with another collider marked as a trigger
    private void OnTriggerEnter2D(Collider2D other)
   {
       // If the melee hits an enemy (assuming enemy has a tag "Enemy")
       if (other.CompareTag("Enemy"))
       {
           // Check if the player has the HealthAndMana script attached
           if (playerHealthAndMana != null && playerHealthAndMana.currentMana < playerHealthAndMana.maxMana)
           {
               // Check if the cooldown has passed
               if (Time.time - lastAttackTime >= manaCooldownTime)
               {
                   // Pulse mana back to the player
                   if (playerHealthAndMana.currentMana < playerHealthAndMana.maxMana)
                   {
                       playerHealthAndMana.currentMana += 1;  // Add mana to the player
                       Debug.Log("Melee hit an enemy. Mana pulsed! Current Mana: " + playerHealthAndMana.currentMana);

                       // Optionally, destroy or deactivate the melee object after hitting an enemy
                       Destroy(gameObject);

                       // Update the time of the last successful mana heal
                       lastAttackTime = Time.time;
                   }
               }
               else
               {
                   Debug.Log("Mana heal is on cooldown.");
               }
           }
       }
   }
}


