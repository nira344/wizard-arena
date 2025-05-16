using UnityEngine;

public class Miasma : MonoBehaviour
{
    public int damagePerSecond = 1;
    public float duration = 5f;
    public float tickRate = 1f;

    private float lifeTimer;
    private float tickTimer;

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        tickTimer += Time.deltaTime;

        if (lifeTimer >= duration)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (tickTimer >= tickRate && other.CompareTag("Player"))
        {
            HealthAndMana health = other.GetComponent<HealthAndMana>();
            if (health != null)
                health.TakeDamage(damagePerSecond);

            tickTimer = 0f;
        }
    }
}
