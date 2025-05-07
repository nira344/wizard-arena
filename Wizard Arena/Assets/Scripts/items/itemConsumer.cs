using UnityEngine;

public class ItemConsumer : MonoBehaviour
{
    private itemSlot slot;
    private InventoryManager inventoryManager;
    private GameObject player;

    //private static bool isConsumingThisFrame = false;  Never used

    void Start()
    {
        slot = GetComponent<itemSlot>();
        inventoryManager = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TryConsume()
    {
        var usable = GetComponent<IUsableItem>();
        if (usable != null)
        {
            Debug.Log($"Consuming {slot.itemName}");
            usable.Use(player);
            slot.quantity--;

            if (slot.quantity <= 0)
                slot.ClearSlot();
            else
                slot.UpdateQuantityText();
        }
    }
}
