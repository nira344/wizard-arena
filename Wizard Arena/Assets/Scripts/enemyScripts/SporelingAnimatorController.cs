using UnityEngine;
using System.Collections;

public class SporelingAnimationScript : MonoBehaviour
{
    private Animator animator;
    private enemyGround groundAI;
    private EnemyAttack attackScript;

    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        groundAI = GetComponent<enemyGround>();
        attackScript = GetComponent<EnemyAttack>();
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
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackScript.attackCooldown);
        isAttacking = false;
    }
}
