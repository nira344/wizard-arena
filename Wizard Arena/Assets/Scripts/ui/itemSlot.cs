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
    public string itemDescription;

    //====== ITEM SLOT UI ======//
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    //====== ITEM DESCRIPTION SLOT ======//
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

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

    public void AddItem(string itemName, int addedQuantity, Sprite itemSprite, string itemDescription)
    {

        this.itemName = itemName;
        this.quantity = addedQuantity;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;

        isFull = true;
        itemImage.sprite = itemSprite;

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
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;

    }

}
