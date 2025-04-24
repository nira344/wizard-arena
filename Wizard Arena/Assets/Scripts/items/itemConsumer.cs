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
            Debug.Log("ItemConsumer: F key pressed on selected full slot.");
            TryConsume();
        }
    }

    public void TryConsume()
    {
        var usable = GetComponent<IUsableItem>();
        if (usable != null)
        {
            Debug.Log("ItemConsumer: Found IUsableItem, consuming...");
            usable.Use(player);
            slot.quantity--;

            if (slot.quantity <= 0)
            {
                Debug.Log("ItemConsumer: Slot now empty. Clearing slot.");
                slot.ClearSlot();
            }
            else
            {
                Debug.Log("ItemConsumer: Updated quantity to " + slot.quantity);
                slot.UpdateQuantityText();
            }
        }
        else
        {
            Debug.LogWarning("ItemConsumer: No IUsableItem found on this GameObject.");
        }
    }
}
