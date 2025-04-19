using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public itemSlot[] itemSlot;

    public TextMeshProUGUI winText;  // Ensure winText is linked to the UI TextMeshPro component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Optionally initialize other components or settings
        winText.gameObject.SetActive(false);  // Hiding winText at start, just in case
    }

    // Update is called once per frame
    void Update()
    {
        if (!winText.IsActive())  // Ensure the win text is not showing before toggling the menu
        {
            if (Input.GetKeyDown(KeyCode.Tab) && menuActivated)
            {
                Time.timeScale = 1;  // Unfreeze time when closing inventory
                InventoryMenu.SetActive(false);
                menuActivated = false;
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && !menuActivated)
            {
                Time.timeScale = 0;  // Freeze time when inventory is opened
                InventoryMenu.SetActive(true);
                menuActivated = true;
            }
        }
    }

    // Add an item to the inventory
    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        // Loop through each item slot
        for (int i = 0; i < itemSlot.Length; i++)
        {
            // Check for an empty slot
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite);
                return;  // Stop after adding to the first empty slot
            }
        }

        // Optionally, you can handle the case where no empty slots are available
        Debug.LogWarning("No empty slot found for " + itemName);
    }

    // Deselect all slots
    public void DeselectAllSlots()
    {
        foreach (var slot in itemSlot)
        {
            slot.selectedShader.SetActive(false);
            slot.thisItemSelected = false;
        }
    }
}
