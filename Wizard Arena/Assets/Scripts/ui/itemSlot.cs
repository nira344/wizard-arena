using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class itemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;
    public GameObject player;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        Transform child = transform.Find("SelectedPanel");
        if (child != null) selectedShader = child.gameObject;

        child = transform.Find("QuantityText");
        if (child != null) quantityText = child.GetComponent<TMP_Text>();

        child = transform.Find("ItemImage");
        if (child != null) itemImage = child.GetComponent<Image>();
    }

    public void AddItem(string itemName, int addedQuantity, Sprite itemSprite, string itemDescription)
    {
        this.itemName = itemName;
        this.quantity = addedQuantity;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;

        isFull = true;
        itemImage.sprite = itemSprite;
        UpdateQuantityText();
        quantityText.enabled = true;
        itemImage.enabled = true;
    }

    public void ClearSlot()
    {
        itemName = "";
        quantity = 0;
        itemSprite = emptySprite;
        itemDescription = "";

        itemImage.sprite = emptySprite;
        quantityText.text = "";
        itemImage.enabled = false;
        quantityText.enabled = false;

        isFull = false;
        thisItemSelected = false;
        selectedShader.SetActive(false);
    }

    public void UpdateQuantityText()
    {
        quantityText.text = quantity.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;

            if (isFull)
            {
                ItemDescriptionNameText.text = itemName;
                ItemDescriptionText.text = itemDescription;
                itemDescriptionImage.sprite = itemSprite != null ? itemSprite : emptySprite;
            }
            else
            {
                ItemDescriptionNameText.text = "";
                ItemDescriptionText.text = "";
                itemDescriptionImage.sprite = emptySprite;
            }
        }
    }

    public void ConfigureSlotForItem(IUsableItem usable)
    {
        var consumer = GetComponent<ItemConsumer>();
        if (consumer != null)
            consumer.enabled = true;
    }

    public void ActivateSelectedItem()
    {
        if (TryGetComponent<ItemConsumer>(out var consumer) && consumer.enabled)
        {
            consumer.TryConsume();
        }
    }
}
