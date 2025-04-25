using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    public int storedSouls;

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

    public void OpenChest()
    {
        SoulManager.Instance.AddSouls(storedSouls);

        foreach (var item in itemDrops)
        {
            if (item != null)
            {
                Vector3 dropOffset = Random.insideUnitSphere * 0.5f;
                dropOffset.y = Mathf.Abs(dropOffset.y); // ensure spawn above ground
                Instantiate(item, transform.position + dropOffset, Quaternion.identity);
            }
        }

        Destroy(gameObject, 0.3f);
    }

    public void OnMeleeHit()
    {
        OpenChest();
    }
}
