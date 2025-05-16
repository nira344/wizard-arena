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
    public IUsableItem usableItem;

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
        itemImage.color = Color.white; // Ensure full visibility
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
        itemImage.color = new Color(1, 1, 1, 0.3f); // Optional faded look
        quantityText.text = "";
        itemImage.enabled = true; // Keep it enabled with faded appearance
        quantityText.enabled = false;

        isFull = false;
        thisItemSelected = false;
        selectedShader.SetActive(false);

        usableItem = null;

        // Remove any lingering consumer
        var consumer = GetComponent<ItemConsumer>();
        if (consumer) Destroy(consumer);
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
        Debug.Log($"Trying to activate item: {itemName}");

        if (TryGetComponent<ItemConsumer>(out var consumer))
        {
            Debug.Log("Using ItemConsumer...");
            consumer.TryConsume();
        }
        else if (usableItem != null)
        {
            Debug.Log("Using IUsableItem directly...");
            usableItem.Use(player);

            quantity--;
            if (quantity <= 0)
                ClearSlot();
            else
                UpdateQuantityText();
        }
        else
        {
            Debug.LogWarning("No usable component found on item slot.");
        }
    }
}
