using UnityEngine;

public class CrystalMana : MonoBehaviour, IUsableItem
{
    public int increaseAmount = 1;

    public void Use(GameObject player)
    {
        var hm = player.GetComponent<HealthAndMana>();
        if (hm != null)
        {
            hm.IncreaseMaxMana(increaseAmount);
            Debug.Log("Used Crystal Mana! Max MP increased.");
        }
    }
}
