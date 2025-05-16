using UnityEngine;

public class shoot : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject fireballPrefab;
    public GameObject iceshardPrefab;
    public GameObject meleePrefab;

    [Header("Cooldown Times")]
    public float fireballCooldownTime = 1.0f;
    public float iceshardCooldownTime = 0.5f;
    public float meleeCooldownTime = 0.1f;

    [Header("Audio Clips")]
    public AudioClip ice_sound;
    public AudioClip fire_sound;

    [Header("Skill Manager")]
    public SkillMenuManager skillMenuManager;

    private AudioSource audioSource;
    private Animator animator;
    private HealthAndMana statScript;

    private float lastIceTime = -999f;
    private float lastFireTime = -999f;
    private float lastMeleeTime = -999f;

    private ShadowDodge shadowDodge;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        statScript = GetComponent<HealthAndMana>();
        shadowDodge = GetComponent<ShadowDodge>();

        if (statScript == null)
            Debug.LogError("Missing HealthAndMana script!");

        if (skillMenuManager == null)
            Debug.LogError("SkillMenuManager not assigned to shoot script.");

        if (shadowDodge == null)
            Debug.LogError("Missing ShadowDodge script!");
    }

    void Update()
    {
        if (Time.timeScale <= 0 || statScript.currentHealth <= 0) return;

        HandleFireball();
        HandleIceShard();
        HandleMelee();
        HandleShadowDash();
    }

    private void HandleFireball()
    {
        if (Input.GetButtonDown("Fire3") && Time.time - lastFireTime >= fireballCooldownTime)
        {
            //if (!IsUnlocked("Fireball")) return;

            if (statScript.currentMana >= 3)
            {
                statScript.currentMana -= 3;
                Instantiate(fireballPrefab, transform.position, transform.rotation);
                PlaySound(fire_sound);
                TriggerAnimation("CastFire");
            }
            else Debug.Log("Not enough mana for Fireball");

            lastFireTime = Time.time;
        }
    }

    private void HandleIceShard()
    {
        if (Input.GetButtonDown("Fire2") && Time.time - lastIceTime >= iceshardCooldownTime)
        {
            //if (!IsUnlocked("Ice Shard")) return;

            if (statScript.currentMana >= 1)
            {
                statScript.currentMana -= 1;
                Instantiate(iceshardPrefab, transform.position, transform.rotation);
                PlaySound(ice_sound);
                TriggerAnimation("CastIce");
            }
            else Debug.Log("Not enough mana for Ice Shard");

            lastIceTime = Time.time;
        }
    }

    private void HandleMelee()
    {
        if (Input.GetButtonDown("Fire1") && Time.time - lastMeleeTime >= meleeCooldownTime)
        {
            Instantiate(meleePrefab, transform.position, transform.rotation);
            TriggerAnimation("Melee");
            lastMeleeTime = Time.time;
        }
    }

    private void HandleShadowDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //if (!IsUnlocked("Shadow Dash")) return;

            if (!shadowDodge.TryTriggerShadowDash())
            {
                Debug.Log("Shadow Dash failed (cooldown or requirements not met)");
            }
        }
    }

    private bool IsUnlocked(string abilityName)
    {
        if (skillMenuManager == null) return true;

        foreach (SkillButton button in skillMenuManager.skillButtons)
        {
            if (button.abilityName == abilityName && button.isUnlocked)
                return true;
        }

        return false;
    }

    private void TriggerAnimation(string triggerName)
    {
        if (animator != null)
            animator.SetTrigger(triggerName);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
