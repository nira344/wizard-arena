using UnityEngine;

public class enemyAir : MonoBehaviour
{

    public float range;
    public float speed;
    GameObject player;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange())
        {
            // get vector pointing from source to target
            Vector2 direction = player.transform.position - transform.position;

            // change the vector to have a magnitude of 1.0 
            direction.Normalize();

            // restrict to horizontal movement
            direction.y *= 0.5f;

            // scale the vector to our desired speed 
            direction = direction * speed * Time.deltaTime;

            // deal with spell knockback
            if (rb && (rb.linearVelocityX != 0 || rb.linearVelocityY != 0))
            {
                rb.AddForceX(rb.linearVelocityX / -(1 + (Time.deltaTime * 2)));
                rb.AddForceY(rb.linearVelocityY / -(1 + (Time.deltaTime * 2)));
            }

            // move in the direction we have calculated
            transform.Translate(direction);
        }
    }

    public bool playerInRange()
    {
        // subtract source and target positions to get vector between
        Vector2 distanceVector = player.transform.position - transform.position;

        // read the magnitude of the distance between objects
        float distance = distanceVector.magnitude;

        // check to see if this object is within range
        if (distance <= range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
