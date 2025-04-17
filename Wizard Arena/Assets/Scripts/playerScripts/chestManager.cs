using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance;

    public GameObject chestPrefab;
    private GameObject chest;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnChest(Vector3 position, int soulAmount)
    {
        if (chest != null)
        {
            Destroy(chest);
            Debug.Log("Chest destroyed.");
        }

        chest = Instantiate(chestPrefab, position, Quaternion.identity);
        chest.GetComponent<Chest>().Initialize(soulAmount);
    }

    public void ClearChest()
    {
        chest = null;
    }
}
