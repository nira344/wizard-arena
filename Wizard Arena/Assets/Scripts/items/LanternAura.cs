using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class LanternAura2D : MonoBehaviour
{
    public float pushForce = 5f;
    public string enemyTag = "SmallEnemy";

    private void Start()
    {
        CircleCollider2D aura = GetComponent<CircleCollider2D>();
        aura.isTrigger = true;
        aura.radius = 3f; // Adjust the radius to set how far the aura reaches
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 pushDirection = (other.transform.position - transform.position).normalized;
                enemyRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}
