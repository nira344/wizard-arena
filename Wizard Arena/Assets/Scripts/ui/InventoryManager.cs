using UnityEngine;
using TMPro;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public itemSlot[] itemSlot;
    public TextMeshProUGUI winText;
    public itemSlot equipSlot;
    public GameObject player;

    private GameObject beltLantern;

    void Start()
    {
        winText.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");

        // Try to find BeltLantern once during Start
        beltLantern = player.GetComponentsInChildren<Transform>(true)
            .FirstOrDefault(t => t.name.ToLower().Contains("beltlantern"))?.gameObject;

        if (beltLantern == null)
            Debug.LogError("InventoryManager: BeltLantern not found under Player!");
    }

    void Update()
    {
        if (!winText.IsActive())
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                menuActivated = !menuActivated;
                InventoryMenu.SetActive(menuActivated);
                Time.timeScale = menuActivated ? 0 : 1;

                if (menuActivated)
                {
                    DeselectAllSlots();
                    if (itemSlot[0].isFull)
                    {
                        itemSlot[0].selectedShader.SetActive(true);
                        itemSlot[0].thisItemSelected = true;
                        itemSlot[0].ItemDescriptionNameText.text = itemSlot[0].itemName;
                        itemSlot[0].ItemDescriptionText.text = itemSlot[0].itemDescription;
                        itemSlot[0].itemDescriptionImage.sprite = itemSlot[0].itemSprite != null
                            ? itemSlot[0].itemSprite
                            : itemSlot[0].emptySprite;
                    }
                }
            }

            if (menuActivated && Input.GetKeyDown(KeyCode.F))
            {
                foreach (var slot in itemSlot)
                {
                    if (slot.thisItemSelected)
                    {
                        slot.ActivateSelectedItem();
                        break;
                    }
                }
            }
        }

        // Enable or disable BeltLantern based on equipped item
        if (beltLantern != null)
        {
            beltLantern.SetActive(equipSlot.itemName == "Lantern");
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, GameObject usableItemObject)
    {
        foreach (var slot in itemSlot)
        {
            if (slot.isFull && slot.itemName == itemName)
            {
                slot.quantity += quantity;
                slot.UpdateQuantityText();
                return;
            }
        }

        itemSlot emptySlot = itemSlot.FirstOrDefault(s => !s.isFull);

        if (emptySlot != null)
        {
            emptySlot.AddItem(itemName, quantity, itemSprite, itemDescription);

            if (!emptySlot.GetComponent<ItemEquipper>())
                emptySlot.gameObject.AddComponent<ItemEquipper>();
            if (!emptySlot.GetComponent<ItemConsumer>())
                emptySlot.gameObject.AddComponent<ItemConsumer>();

            if (usableItemObject.TryGetComponent<IUsableItem>(out var usableItem))
                emptySlot.ConfigureSlotForItem(usableItem);
            else
                Debug.LogWarning($"AddItem: No IUsableItem found on {usableItemObject.name}");

            return;
        }

        Debug.LogWarning("No empty inventory slot for: " + itemName);
    }

    public void DeselectAllSlots()
    {
        foreach (var slot in itemSlot)
        {
            slot.selectedShader.SetActive(false);
            slot.thisItemSelected = false;
        }

        equipSlot.selectedShader.SetActive(false);
        equipSlot.thisItemSelected = false;
    }

    public void EquipItem(itemSlot fromSlot)
    {
        if (equipSlot.isFull)
        {
            UnequipItem();
        }

        equipSlot.AddItem(fromSlot.itemName, fromSlot.quantity, fromSlot.itemSprite, fromSlot.itemDescription);

        if (fromSlot.TryGetComponent<IUsableItem>(out var usable))
        {
            var usableType = usable.GetType();
            if (!equipSlot.gameObject.GetComponent(usableType))
            {
                equipSlot.gameObject.AddComponent(usableType);
            }
        }

        fromSlot.ClearSlot();
    }

    public void UnequipItem()
    {
        if (equipSlot.isFull)
        {
            AddItem(
                equipSlot.itemName,
                equipSlot.quantity,
                equipSlot.itemSprite,
                equipSlot.itemDescription,
                equipSlot.gameObject
            );

            equipSlot.ClearSlot();
        }
    }
}
