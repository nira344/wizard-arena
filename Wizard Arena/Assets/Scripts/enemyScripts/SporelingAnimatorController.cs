using UnityEngine;
using System.Collections;

public class SporelingAnimationScript : MonoBehaviour
{
    private Animator animator;
    private enemyGround groundAI;
    private EnemyAttack attackScript;
    private Rigidbody2D rb;

    [Header("Attack Jump Settings")]
    public float attackJumpForce = 5f; // You can tweak this in the Inspector

    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        groundAI = GetComponent<enemyGround>();
        attackScript = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovementAnimation();
        HandleAttackAnimation();
    }

    void HandleMovementAnimation()
    {
        if (groundAI != null && animator != null)
        {
            bool shouldWalk = groundAI.range > 0 && groundAI.playerInRange();
            animator.SetBool("isWalking", shouldWalk);
        }
    }

    void HandleAttackAnimation()
    {
        if (attackScript != null && animator != null && attackScript.IsTouchingPlayer() && !isAttacking)
        {
            animator.SetTrigger("Attack");
            PerformAttackJump();
            StartCoroutine(AttackCooldown());
        }
    }

    void PerformAttackJump()
    {
        if (rb != null)
        {
            // Optional: reset vertical velocity before applying force
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * attackJumpForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackScript.attackCooldown);
        isAttacking = false;
    }
}
