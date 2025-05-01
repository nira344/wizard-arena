using UnityEngine;

public class manaSteal : MonoBehaviour
{
    public float steal = 2f;  // Amount of mana to steal per tick
    public float stealCooldown = 1f;  // Time between each mana steal
    private float lastStealTime = 0f;
    private bool touchingPlayer = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (touchingPlayer && Time.time - lastStealTime >= stealCooldown)
        {
            HealthAndMana playerHealthMana = player.GetComponent<HealthAndMana>();
            if (playerHealthMana != null)
            {
                playerHealthMana.DrainMana(steal);  // Call the correct mana-drain method
                lastStealTime = Time.time;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = false;
    }

    public bool IsTouchingPlayer()
    {
        return touchingPlayer;
    }
}
