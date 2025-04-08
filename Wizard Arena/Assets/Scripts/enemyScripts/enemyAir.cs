using UnityEngine;

public class enemyAir : MonoBehaviour
{

    public float range;
    public float speed;
    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            direction.y = 0;

            // scale the vector to our desired speed 
            direction = direction * speed * Time.deltaTime;

            // move in the direction we have calculated
            transform.Translate(direction);
        }
    }

    bool playerInRange()
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
