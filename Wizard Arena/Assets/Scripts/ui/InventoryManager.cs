using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public itemSlot[] itemSlot;
    public TextMeshProUGUI winText;
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

            if (usableItemObject.TryGetComponent<IUsableItem>(out var usable))
            {
                var usableType = usable.GetType();
                var newUsable = (IUsableItem)emptySlot.gameObject.AddComponent(usableType);
                emptySlot.usableItem = newUsable;
                emptySlot.ConfigureSlotForItem(newUsable);
            }

            if (itemName.Contains("Crystal") || itemName.Contains("Potion"))
            {
                if (!emptySlot.GetComponent<ItemConsumer>())
                    emptySlot.gameObject.AddComponent<ItemConsumer>();
            }

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
    }
}
