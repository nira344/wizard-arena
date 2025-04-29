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

    private AudioSource audioSource;
    private Animator animator;
    private HealthAndMana statScript;

    private float lastIceTime = 0f;
    private float lastFireTime = 0f;
    private float lastMeleeTime = 0f;

    private bool isCasting = false;

    void Start()
    {
        // Get own components
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        statScript = GetComponent<HealthAndMana>();

        // Turn off cooldowns
        lastIceTime = (iceshardCooldownTime * -1);
        lastFireTime = (fireballCooldownTime * -1);
        lastMeleeTime = (meleeCooldownTime * -1);

        if (statScript == null)
        {
            Debug.LogError("HealthAndMana script not found on player!");
        }
    }

    void Update()
    {
        if (Time.timeScale <= 0) return;

        HandleIceShard();
        HandleFireball();
        HandleMelee();

        UpdateAnimationState();
    }

    private void HandleIceShard()
    {
        if (Input.GetButtonDown("Fire2") && Time.time - lastIceTime >= iceshardCooldownTime)
        {
            if (statScript != null && statScript.currentMana >= 1)
            {
                statScript.currentMana -= 1;
                Instantiate(iceshardPrefab, transform.position, transform.rotation);
                PlaySound(ice_sound);
                SetAnimationState(7); // Cast Ice Shard
                isCasting = true;
            }
            else
            {
                Debug.Log("Not enough mana for Ice Shard");
            }
            lastIceTime = Time.time;
        }
    }

    private void HandleFireball()
    {
        if (Input.GetButtonDown("Fire3") && Time.time - lastFireTime >= fireballCooldownTime)
        {
            if (statScript != null && statScript.currentMana >= 3)
            {
                statScript.currentMana -= 3;
                Instantiate(fireballPrefab, transform.position, transform.rotation);
                PlaySound(fire_sound);
                SetAnimationState(6); // Cast Fireball
                isCasting = true;
            }
            else
            {
                Debug.Log("Not enough mana for Fireball");
            }
            lastFireTime = Time.time;
        }
    }

    private void HandleMelee()
    {
        if (Input.GetButtonDown("Fire1") && (Time.time - lastMeleeTime) >= meleeCooldownTime)
        {
            Instantiate(meleePrefab, transform.position, transform.rotation);
            SetAnimationState(5); // Attack Animation (you can set your melee anim state here)
            isCasting = true;
            lastMeleeTime = Time.time;
        }
    }

    private void UpdateAnimationState()
    {
        if (statScript != null && statScript.IsDead())
        {
            SetAnimationState(4); // Dead
            return;
        }

        if (isCasting)
        {
            // Casting animations already set, reset after short time if needed
            isCasting = false;
        }
        else
        {
            // Set Idle or Walk based on player movement
            SetAnimationState(0); // Idle
        }
    }

    private void SetAnimationState(int state)
    {
        if (animator != null)
        {
            animator.SetInteger("State", state);
        }
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
