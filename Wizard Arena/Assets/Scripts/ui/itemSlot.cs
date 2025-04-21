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
    }

    void Update()
    {
        if (thisItemSelected && Input.GetKeyDown(KeyCode.F) && isFull)
        {
            var usable = GetComponent<IUsableItem>();
            Debug.Log("Trying to use: " + itemName + " | Usable: " + (usable != null));

            if (usable != null)
            {
                usable.Use(player);
                quantity--;

                if (quantity <= 0)
                {
                    ClearSlot();
                }
                else
                {
                    quantityText.text = quantity.ToString();
                }
            }
            else
            {
                Debug.LogWarning("Item does not implement IUsableItem: " + itemName);
            }
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;

            ItemDescriptionNameText.text = itemName;
            ItemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite != null ? itemSprite : emptySprite;
        }
    }
}
