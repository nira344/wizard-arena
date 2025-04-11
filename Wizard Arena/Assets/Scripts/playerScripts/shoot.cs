using UnityEngine;


public class shoot : MonoBehaviour
{
   public GameObject fireballPrefab;
   public GameObject iceshardPrefab;
   public GameObject meleePrefab;


   public float fireballCooldownTime = 0.5f;  // Cooldown time for fireball
   public float iceshardCooldownTime = 0.2f;  // Cooldown time for iceshard
   private float lastAttackTime = 0f;


   void Start()
   {
       // Log initial mana and collision status at the start
       HealthAndMana statScript = GetComponent<HealthAndMana>();
       if (statScript != null)
       {
           Debug.Log("Starting Mana: " + statScript.currentMana + "/" + statScript.maxMana);
       }
   }


   void Update()
   {
       // Handling Ice Shard Attack (Fire3)
       if (Input.GetButtonDown("Fire2"))
       {
           // Check if the cooldown has passed
           if (Time.time - lastAttackTime >= iceshardCooldownTime)
           {
               HealthAndMana statScript = GetComponent<HealthAndMana>();
               if (statScript != null)
               {
                   if (statScript.currentMana + statScript.currentHealth > 1)
                   {
                       Debug.Log("Ice Shard Attack: Mana before: " + statScript.currentMana);
                       statScript.currentMana -= 1;
                       Debug.Log("Ice Shard Attack: Mana after: " + statScript.currentMana);
                       var iceShard = Instantiate(iceshardPrefab, transform.position, transform.rotation);  // Instantiate the iceshard
                   }
                   else
                   {
                       Debug.Log("Not enough mana for Ice Shard");
                   }
               }
               else
               {
                   var iceShard = Instantiate(iceshardPrefab, transform.position, transform.rotation);  // Instantiate the iceshard
               }
              
               // Update the time of last attack
               lastAttackTime = Time.time;
           }
           else
           {
               Debug.Log("iceshard attack is on cooldown.");
           }
       }


       // Handling Fireball Attack (Fire2)
       if (Input.GetButtonDown("Fire3"))
       {
           // Check if the cooldown has passed
           if (Time.time - lastAttackTime >= fireballCooldownTime)
           {
               HealthAndMana statScript = GetComponent<HealthAndMana>();
               if (statScript != null)
               {
                   if (statScript.currentMana + statScript.currentHealth > 3)
                   {
                       Debug.Log("Fireball Attack: Mana before: " + statScript.currentMana);
                       statScript.currentMana -= 3;
                       Debug.Log("Fireball Attack: Mana after: " + statScript.currentMana);
                       var fireBall = Instantiate(fireballPrefab, transform.position, transform.rotation);  // Instantiate the fireball
                   }
                   else
                   {
                       Debug.Log("Not enough mana for Fireball");
                   }
               }
               else
               {
                   var fireBall = Instantiate(fireballPrefab, transform.position, transform.rotation);  // Instantiate the fireball
               }


               // Update the time of last attack
               lastAttackTime = Time.time;
           }
           else
           {
               Debug.Log("fireball attack is on cooldown.");
           }
       }


       // Handling Melee Attack (Fire1)
       if (Input.GetButtonDown("Fire1"))
       {
           // Perform melee attack
           HealthAndMana statScript = GetComponent<HealthAndMana>();
           Debug.Log("Melee Attack Button Pressed");
           var melee = Instantiate(meleePrefab, transform.position, transform.rotation);  // Instantiate the melee


           // Add 1 mana for melee attack
           if (statScript != null)
           {
               Debug.Log("Melee Attack: Mana after: " + statScript.currentMana);
           }


           // Update the time of last attack
           lastAttackTime = Time.time;
       }
   }
}




