using UnityEngine;

public class shoot : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject meleePrefab;

    [Header("Cooldown Times")]
    public float meleeCooldownTime = 0.1f;

    [Header("Audio Clips")]
    public AudioClip ice_sound;
    public AudioClip fire_sound;

    private AudioSource audioSource;
    private Animator animator;
    private HealthAndMana statScript;

    private float lastIceTime = 0f;
    private float lastFireTime = 0f;
    private float lastMeleeTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        statScript = GetComponent<HealthAndMana>();

        if (statScript == null)
        {
            Debug.LogError("HealthAndMana script not found on player!");
        }
    }

    void Update()
    {
        if (Time.timeScale <= 0 || statScript.currentHealth == 0) return;

        // First spell (E key)
        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastFireTime >= GetCooldown(SpellMenuManager.Instance.equippedPrimarySpell))
        {
            TryCastSpell(SpellMenuManager.Instance.equippedPrimarySpell, ref lastFireTime);
        }

        // Second spell (R key)
        if (Input.GetKeyDown(KeyCode.R) && Time.time - lastIceTime >= GetCooldown(SpellMenuManager.Instance.equippedSecondarySpell))
        {
            TryCastSpell(SpellMenuManager.Instance.equippedSecondarySpell, ref lastIceTime);
        }

        HandleMelee();
    }

    private float GetCooldown(Spell spell)
    {
        return spell != null ? spell.cooldown : 1f;
    }

    private void TryCastSpell(Spell spell, ref float lastTime)
    {
        if (spell == null || spell.prefab == null) return;

        if (statScript.currentMana >= spell.manaCost)
        {
            statScript.currentMana -= Mathf.RoundToInt(spell.manaCost);
            Instantiate(spell.prefab, transform.position, transform.rotation);
            TriggerAnimation("Cast" + spell.spellType.ToString());
            lastTime = Time.time;
            PlaySound(spell.spellType);
        }
        else
        {
            Debug.Log("Not enough mana for " + spell.spellName);
        }
    }

    private void HandleMelee()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastMeleeTime >= meleeCooldownTime)
        {
            Instantiate(meleePrefab, transform.position, transform.rotation);
            TriggerAnimation("Melee");
            lastMeleeTime = Time.time;
        }
    }

    private void TriggerAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    private void PlaySound(Spell.SpellType type)
    {
        if (audioSource == null) return;

        switch (type)
        {
            case Spell.SpellType.Fireball:
                audioSource.PlayOneShot(fire_sound);
                break;
            case Spell.SpellType.IceShard:
                audioSource.PlayOneShot(ice_sound);
                break;
        }
    }
}
