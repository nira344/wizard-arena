using UnityEngine;


public class melee : MonoBehaviour
{
   public int damage = 1;  // Configurable from the Inspector
   private float destroyDelay = 0.25f; // Time before destroying the object
   private float timeSinceCreation; // Time elapsed since the object was created
   private Collider2D playerCollider; // Reference to the player's collider
   private Vector2 attackDirection; // Direction of the melee attack
   public float attackRange = 1.5f; // Distance at which the melee attack should appear from the player
   private HealthAndMana playerHealthAndMana;


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


       // Get the attack direction based on player input
       SetAttackDirection();


       // Place the melee attack farther from the character based on the attack range
       PositionAttackObject();
   }


   void Update()
   {
       // If the time since creation is greater than or equal to the destroyDelay, destroy the object
       if (Time.time - timeSinceCreation >= destroyDelay)
       {
           Destroy(gameObject);  // Destroy the object
       }
   }


   private void SetAttackDirection()
   {
       // Get player input direction (arrow keys or joystick)
       float horizontal = Input.GetAxisRaw("Horizontal");
       float vertical = Input.GetAxisRaw("Vertical");


       // Determine the direction of the melee attack
       attackDirection = new Vector2(horizontal, vertical);


       // If no input is given, use the direction the player is facing (for example, assuming right is default)
       if (attackDirection == Vector2.zero)
       {
           attackDirection = Vector2.right; // Default to right if no input is provided
       }


       // Rotate the melee attack based on the input direction
       RotateMeleeAttack();
   }


   private void RotateMeleeAttack()
   {
       // Calculate the angle of the attack direction in degrees
       float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;


       // Apply the rotation to the melee attack object
       transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));  // Corrected rotation
   }


   private void PositionAttackObject()
   {
       // Place the melee attack object a specified distance away from the player in the attack direction
       transform.position = (Vector2)transform.position + attackDirection.normalized * attackRange;
   }


   private void OnCollisionEnter2D(Collision2D collision)
   {
       if (collision.gameObject.tag == "Solid")
       {
           Debug.Log("Hit non-enemy " + collision.gameObject);
           GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
       }


       // Check if the collision is with an object tagged "Enemy"
       if (collision.gameObject.tag.Equals("Enemy"))
       {


           Debug.Log("Hit enemy " + collision.gameObject);
           var healthComponent = collision.gameObject.GetComponent<enemyHealth>();


           if (healthComponent != null)
           {
               healthComponent.TakeDamage(damage);  // Use the damage variable
           }


           GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
       }
   }


    // This will be called when the melee object collides with another collider marked as a trigger
   void OnTriggerEnter(Collider other)
   {
       // If the melee hits an enemy (assuming enemy has a tag "Enemy")
       if (other.CompareTag("Enemy"))
       {
           // Check if the player has the HealthAndMana script attached
           if (playerHealthAndMana != null && playerHealthAndMana.currentMana < playerHealthAndMana.maxMana)
           {
               // Pulse mana back to the player
               playerHealthAndMana.currentMana += 1;
               Debug.Log("Melee hit an enemy. Mana pulsed! Current Mana: " + playerHealthAndMana.currentMana);


               // Optionally, destroy or deactivate the melee object after hitting an enemy
               Destroy(gameObject);
           }
       }
   }
}
