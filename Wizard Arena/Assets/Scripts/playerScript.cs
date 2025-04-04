using UnityEngine;

public class playerScript : MonoBehaviour
{
    public float speed;

    public float jump = 300.0f;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);

    }
    void OnCollisionEnter2D(Collision2D otherObject)
    {
        // if we have hit the hidden platform
        if (otherObject.gameObject.name.Equals("Black"))
        {
            // if user has pressed the Space jump
            if (Input.GetKeyDown("space"))
            {
                // add upwards force
                rb.AddForce(new Vector2(rb.linearVelocityX, jump));

            }

        }
    }
}
