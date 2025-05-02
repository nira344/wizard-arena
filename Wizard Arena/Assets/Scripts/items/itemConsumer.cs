using UnityEngine;
using System.Collections;

public class ItemConsumer : MonoBehaviour
{
    private itemSlot slot;
    private InventoryManager inventoryManager;
    private GameObject player;
    private bool isConsuming = false;

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

        // Only let the SELECTED slot consume
        if (!slot.thisItemSelected || !slot.isFull) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryConsume();
        }
    }

    public void TryConsume()
    {
        if (isConsuming) return;

        isConsuming = true;

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

        StartCoroutine(ResetConsumptionCooldown());
    }

    private IEnumerator ResetConsumptionCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        isConsuming = false;
    }
}
