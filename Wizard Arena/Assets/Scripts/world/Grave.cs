using UnityEngine;

public class Grave : MonoBehaviour
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
            Debug.Log($"Player reclaimed {storedSouls} souls!");
        }
        Destroy(gameObject);
    }
}