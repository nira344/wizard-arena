using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public itemSlot[] itemSlot;

    public TextMeshProUGUI winText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!winText.IsActive())
        {
            if (Input.GetKeyDown(KeyCode.Tab) && menuActivated)
            {
                Time.timeScale = 1;
                InventoryMenu.SetActive(false);
                menuActivated = false;
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && !menuActivated)
            {
                Time.timeScale = 0;
                InventoryMenu.SetActive(true);
                menuActivated = true;
            }
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite);
                return;
            }

        }
    }

}
