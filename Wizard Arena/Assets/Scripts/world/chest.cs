using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    [SerializeField] private int storedSouls;

    [SerializeField] private List<GameObject> itemDrops = new List<GameObject>();

    public void Initialize(int soulAmount, List<GameObject> items = null)
    {
        storedSouls = soulAmount;

        // If items are passed in manually (runtime), use them
        if (items != null && items.Count > 0)
        {
            itemDrops = items;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Manual chest open test");
            OpenChest();
        }
    }

    public void OpenChest()
    {
        Debug.Log("Opening chest: Dropping " + itemDrops.Count + " items, Souls: " + storedSouls);

        SoulManager.Instance.AddSouls(storedSouls);

        foreach (var item in itemDrops)
        {
            if (item != null)
            {
                Vector3 dropOffset = Random.insideUnitSphere * 0.5f;
                dropOffset.y = Mathf.Abs(dropOffset.y);
                var drop = Instantiate(item, transform.position + dropOffset, Quaternion.identity);
                Debug.Log("Dropped item: " + drop.name);
            }
        }

        Destroy(gameObject, 0.5f);
    }


    public void OnMeleeHit()
    {
        OpenChest();
    }
}
