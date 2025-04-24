using UnityEngine;

public class CrystalHeart : MonoBehaviour, IUsableItem 
{
    public int increaseAmount = 1;

    public void Use(GameObject player)
    {
        var hm = player.GetComponent<HealthAndMana>();
        if (hm != null)
        {
            hm.IncreaseMaxHealth(increaseAmount);
            Debug.Log("Used Crystal Heart! Max HP increased.");
        }
    }
}
