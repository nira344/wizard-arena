using UnityEngine;

public class ItemConsumer : MonoBehaviour
{
    private itemSlot slot;
    private InventoryManager inventoryManager;
    private GameObject player;

    void Start()
    {
        slot = GetComponent<itemSlot>();
        if (slot == null)
        {
            Debug.LogError("ItemConsumer: Missing itemSlot on GameObject.");
            return;
        }

        inventoryManager = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("ItemConsumer: InventoryManager not found.");
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("ItemConsumer: Player not found.");
        }
    }

    void Update()
    {
        if (slot == null || inventoryManager == null || player == null) return;

        if (slot.thisItemSelected && Input.GetKeyDown(KeyCode.F) && slot.isFull)
        {
            TryConsume();
        }
    }

    public void TryConsume()
    {
        var usable = GetComponent<IUsableItem>();
        if (usable != null)
        {
            usable.Use(player);
            slot.quantity--;

            if (slot.quantity <= 0)
                slot.ClearSlot();
            else
                slot.UpdateQuantityText();
        }
    }
}