using UnityEngine;

public class ItemEquipper : MonoBehaviour
{
    private itemSlot slot;
    private InventoryManager inventoryManager;

    void Start()
    {
        slot = GetComponent<itemSlot>();
        if (slot == null)
        {
            Debug.LogError("ItemEquipper: Missing itemSlot on GameObject.");
            return;
        }

        inventoryManager = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("ItemEquipper: InventoryManager not found.");
        }
    }

    void Update()
    {
        if (slot == null || inventoryManager == null) return;

        if (slot.thisItemSelected && Input.GetKeyDown(KeyCode.F) && slot.isFull)
        {
            if (slot == inventoryManager.equipSlot)
            {
                TryUnequip();
            }
            else if (GetComponent<IUsableItem>() != null)
            {
                TryEquip();
            }
        }
    }

    public void TryEquip()
    {
        if (!slot.isFull || slot == inventoryManager.equipSlot)
            return;

        inventoryManager.EquipItem(slot);
    }

    public void TryUnequip()
    {
        if (slot == inventoryManager.equipSlot)
        {
            inventoryManager.UnequipItem();
        }
    }
}
