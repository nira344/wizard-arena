using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class itemSlot : MonoBehaviour, IPointerClickHandler
{
    //====== ITEM DATA ======//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;

    //====== ITEM SLOT UI ======//
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    void Start()
    {
        GameObject inventoryCanvas = GameObject.Find("InventoryCanvas");
        if (inventoryCanvas != null)
        {
            inventoryManager = inventoryCanvas.GetComponent<InventoryManager>();
        }
        else
        {
            Debug.LogError("InventoryCanvas not found in scene!");
        }
    }

    public void AddItem(string itemName, int addedQuantity, Sprite itemSprite)
    {
        if (isFull && this.itemName == itemName)
        {
            quantity += addedQuantity;
        }
        else
        {
            this.itemName = itemName;
            this.quantity = addedQuantity;
            this.itemSprite = itemSprite;
            isFull = true;
            itemImage.sprite = itemSprite;
        }

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }

    public void OnLeftClick()
    {
        if (inventoryManager != null)
        {
            inventoryManager.DeselectAllSlots();
        }

        selectedShader.SetActive(true);
        thisItemSelected = true;

    }

}
