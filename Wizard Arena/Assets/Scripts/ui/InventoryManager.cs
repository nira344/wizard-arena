using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public itemSlot[] itemSlot;
    public TextMeshProUGUI winText;

    void Start()
    {
        winText.gameObject.SetActive(false);
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
                        itemSlot[0].itemDescriptionImage.sprite = itemSlot[0].itemSprite != null ? itemSlot[0].itemSprite : itemSlot[0].emptySprite;
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
                slot.UpdateQuantityText(); // ✅ Use the public method instead of accessing private field
                return;
            }
        }

        // If not found, add to a new slot
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);

                // Copy usable item script from prefab to slot
                if (usableItemObject.TryGetComponent<IUsableItem>(out var usable))
                {
                    System.Type usableType = usable.GetType();
                    if (!itemSlot[i].gameObject.GetComponent(usableType))
                    {
                        itemSlot[i].gameObject.AddComponent(usableType);
                    }
                }

                return;
            }
        }

        Debug.LogWarning("No empty inventory slot found for: " + itemName);
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
