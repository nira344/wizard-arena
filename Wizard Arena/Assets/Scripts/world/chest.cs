using UnityEngine;

public class Chest : MonoBehaviour
{
    public int storedSouls;

    public void Initialize(int souls)
    {
        storedSouls = souls;
    }

    public void OnMeleeHit()
    {
        if (SoulManager.Instance != null)
        {
            SoulManager.Instance.AddSouls(storedSouls);
            SoulManager.Instance.UpdateSoulText();
            Debug.Log($"Chest destroyed! Player gained {storedSouls} souls!");
        }
        Destroy(gameObject);
    }
}
