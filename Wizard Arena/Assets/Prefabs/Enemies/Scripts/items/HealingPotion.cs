using UnityEngine;

public class HealingPotion : MonoBehaviour, IUsableItem
{
    public int healAmount = 3;

    public void Use(GameObject player)
    {
        var hm = player.GetComponent<HealthAndMana>();
        if (hm != null)
        {
            hm.Heal(healAmount);
            Debug.Log("Used Healing Potion! Healed " + healAmount + " HP.");
        }
    }
}
