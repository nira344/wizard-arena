using UnityEngine;

public class EnemySpellCaster : MonoBehaviour
{
    [Header("Spell Configuration")]
    public GameObject spell;
    public float spellCooldown = 2f;
    public float castRange = 6f;

    private float cooldownTimer;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (PlayerInRange() && cooldownTimer <= 0f)
        {
            CastSpell();
            cooldownTimer = spellCooldown;
        }
    }

    private bool PlayerInRange()
    {
        return Vector2.Distance(transform.position, player.transform.position) <= castRange;
    }

    private void CastSpell()
    {
        Instantiate(spell, transform.position, Quaternion.identity);
    }
}
