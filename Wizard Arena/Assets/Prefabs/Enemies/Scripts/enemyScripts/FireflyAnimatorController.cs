using UnityEngine;

public class FireflyAnimatorController : MonoBehaviour
{
    private Animator animator;
    private enemyAir airAI;
    private manaSteal stealScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        airAI = GetComponent<enemyAir>();
        stealScript = GetComponent<manaSteal>();
    }

    void Update()
    {
        HandleMovementAnimation();
    }

    void HandleMovementAnimation()
    {
        if (airAI != null)
        {
            bool shouldFly = airAI.range > 0 && airAI.playerInRange();
            animator.SetBool("isFlying", shouldFly);
        }
    }
}
