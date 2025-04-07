using UnityEngine;

public class HealthAndMana : MonoBehaviour
{
    public int maxHealth = 10;  // Starting HP
    public int maxMana = 20;    // Starting Mana (shield)
    public int currentHealth;
    public int currentMana;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        // Ensure that mana is never less than health on start
        BalanceHealthAndMana();
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure that mana is always equal to or greater than health
        BalanceHealthAndMana();
    }

    // Method to take damage
    public void TakeDamage(float amount)
    {
        // If there's mana, damage the mana first (with 1.5x damage)
        if (currentMana > 0)
        {
            // Apply damage to mana (1.5x multiplier)
            float manaDamage = Mathf.Min(amount * 1.5f, currentMana);
            currentMana -= Mathf.FloorToInt(manaDamage);

            // If mana is depleted, continue applying damage to health
            if (currentMana < 0)
            {
                float remainingDamage = Mathf.Abs(currentMana);
                currentMana = 0;
                currentHealth -= Mathf.FloorToInt(remainingDamage);
            }
        }
        else
        {
            // If no mana, apply damage directly to health
            currentHealth -= Mathf.FloorToInt(amount);
        }

        // Prevent health from going below zero
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Handle death logic here
            Debug.Log("Player is dead!");
        }

        // Prevent mana from going below zero
        if (currentMana < 0)
        {
            currentMana = 0;
        }

        // Ensure that HP and Mana balance out
        BalanceHealthAndMana();
    }

    // Method to balance health and mana (can't have more HP than Mana and less Mana than HP)
    void BalanceHealthAndMana()
    {
        // If health is greater than mana, equalize them
        if (currentHealth > currentMana)
        {
            currentHealth = currentMana;
        }
        // If mana is less than health, equalize them
        else if (currentMana < currentHealth)
        {
            currentMana = currentHealth;
        }
    }

    // Method to handle casting a spell
    public void CastSpell(float manaCost)
    {
        if (currentMana >= manaCost)
        {
            // If the player has enough mana, reduce both mana and health
            currentMana -= Mathf.FloorToInt(manaCost);
            currentHealth -= Mathf.FloorToInt(manaCost * 0.5f); // Reduce health by half the mana cost

            // Ensure health and mana don't drop below zero
            if (currentMana < 0)
            {
                currentMana = 0;
            }

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
        }
        else
        {
            Debug.Log("Not enough mana to cast this spell!");
        }

        // Ensure that HP and Mana balance out after casting
        BalanceHealthAndMana();
    }
}