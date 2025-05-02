using UnityEngine;
using System.Collections.Generic;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance;

    public GameObject chestPrefab;
    private GameObject chest;

    [Header("Item Drop Settings")]
    public List<GameObject> possibleItemDrops; // Assign item prefabs in Inspector
    public int maxItemsToDrop = 2; // Max items dropped per chest

    void Awake()
    {
        Instance = this;
    }

    public void SpawnChest(Vector3 position, int soulAmount)
    {
        if (chest != null)
        {
            Destroy(chest);
            Debug.Log("Old chest destroyed.");
        }

        chest = Instantiate(chestPrefab, position, Quaternion.identity);

        // Initialize chest with souls and possible item drops
        Chest chestComponent = chest.GetComponent<Chest>();
        if (chestComponent != null)
        {
            List<GameObject> itemsToDrop = new List<GameObject>();

            int dropCount = Mathf.Min(maxItemsToDrop, possibleItemDrops.Count);
            for (int i = 0; i < dropCount; i++)
            {
                int randomIndex = Random.Range(0, possibleItemDrops.Count);
                itemsToDrop.Add(possibleItemDrops[randomIndex]);
            }

            chestComponent.Initialize(soulAmount, itemsToDrop);
        }
    }

    public void ClearChest()
    {
        chest = null;
    }
}
