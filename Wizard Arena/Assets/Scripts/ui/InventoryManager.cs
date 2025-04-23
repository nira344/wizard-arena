using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public itemSlot[] itemSlot;
    public TextMeshProUGUI winText;
    public itemSlot equipSlot;
    public GameObject player;

    void Start()
    {
        winText.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!winText.IsActive())
        {
            // Toggle inventory menu
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

            // Use selected item
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

        // 🔦 Attach and activate LanternObject if equipped
        GameObject lanternObject = GameObject.Find("LanternObject");
        if (equipSlot.itemName == "Lantern")
        {
            if (lanternObject != null && !lanternObject.activeSelf)
            {
                lanternObject.SetActive(true);
            }

            if (lanternObject != null && lanternObject.transform.parent != player.transform)
            {
                lanternObject.transform.SetParent(player.transform);
                lanternObject.transform.localPosition = new Vector3(0.5f, -0.6f, 0); // adjust as needed for belt
            }
        }
        else
        {
            if (lanternObject != null)
            {
                lanternObject.SetActive(false);
            }
        }

        // 🌟 Activate or deactivate the LanternEffect
        Transform lanternEffect = player.transform.Find("LanternEffect");
        if (lanternEffect != null)
        {
            lanternEffect.gameObject.SetActive(equipSlot.itemName == "Lantern");
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

        itemSlot emptySlot = null;
        foreach (var s in itemSlot)
        {
            if (!s.isFull)
            {
                emptySlot = s;
                break;
            }
        }

        if (emptySlot != null)
        {
            emptySlot.AddItem(itemName, quantity, itemSprite, itemDescription);

            if (!emptySlot.GetComponent<ItemEquipper>())
                emptySlot.gameObject.AddComponent<ItemEquipper>();
            if (!emptySlot.GetComponent<ItemConsumer>())
                emptySlot.gameObject.AddComponent<ItemConsumer>();

            if (usableItemObject.TryGetComponent<IUsableItem>(out var usableItem))
                emptySlot.ConfigureSlotForItem(usableItem);

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

            var lantern = GameObject.Find("LanternObject");
            if (lantern != null)
            {
                lantern.SetActive(false);
            }
        }
    }
}
