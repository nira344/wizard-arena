using UnityEngine;

public class enemyGroundTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public float range = 10f;
    public float minTeleportDistance = 2f;  // Minimum distance from player
    public float maxTeleportDistance = 4f;  // Maximum distance from player
    public float teleportCooldown = 2f;

    [Header("Audio")]
    public AudioSource teleportSound;

    private GameObject player;
    private float cooldownTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (playerInRange() && cooldownTimer <= 0f)
        {
            TeleportNearPlayer();
            cooldownTimer = teleportCooldown;
        }
    }

    bool playerInRange()
    {
        return Vector2.Distance(transform.position, player.transform.position) <= range;
    }

    void TeleportNearPlayer()
    {
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Choose a direction (left or right) to appear from
        float side = Random.value < 0.5f ? -1f : 1f;

        // Choose a random distance between min and max
        float teleportOffset = Random.Range(minTeleportDistance, maxTeleportDistance);

        // Calculate the 2D target position relative to player
        Vector2 flatTargetPosition = (Vector2)player.transform.position + new Vector2(teleportOffset * side, 0);

        // Final teleport position with fixed Z
        Vector3 targetPosition = new Vector3(flatTargetPosition.x, flatTargetPosition.y, -5f);

        // Face the player
        if (player.transform.position.x > transform.position.x)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        // Teleport the enemy
        transform.position = targetPosition;

        if (teleportSound)
            teleportSound.Play();
    }
}
