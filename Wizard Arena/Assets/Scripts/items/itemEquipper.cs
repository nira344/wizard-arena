using UnityEngine;

public class ItemEquipper : MonoBehaviour
{
    private itemSlot slot;
    private InventoryManager inventoryManager;

    void Start()
    {
        slot = GetComponent<itemSlot>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void TryEquip()
    {
        if (!slot.isFull || slot == inventoryManager.equipSlot)
            return;

        if (slot.GetComponent<IUsableItem>() != null)
        {
            inventoryManager.EquipItem(slot);
        }
    }

    public void TryUnequip()
    {
        if (slot == inventoryManager.equipSlot)
        {
            inventoryManager.UnequipItem();
        }
    }
}
