using UnityEngine;

public class ItemConsumer : MonoBehaviour
{
    private itemSlot slot;
    private InventoryManager inventoryManager;
    private GameObject player;

    void Start()
    {
        slot = GetComponent<itemSlot>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TryConsume()
    {
        if (!slot.isFull) return;

        var usable = GetComponent<IUsableItem>();
        if (usable != null)
        {
            usable.Use(player);
            slot.quantity--;

            if (slot.quantity <= 0) slot.ClearSlot();
            else slot.UpdateQuantityText();
        }
    }
}
