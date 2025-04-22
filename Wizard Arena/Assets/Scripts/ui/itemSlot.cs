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
           if (this == inventoryManager.equipSlot)
           {
               inventoryManager.UnequipItem();
               return;
           }


           var usable = GetComponent<IUsableItem>();
           if (usable != null && inventoryManager.equipSlot != this)
           {
               inventoryManager.EquipItem(this);
               return;
           }


           usable?.Use(player);
           quantity--;


           if (quantity <= 0) ClearSlot();
           else UpdateQuantityText();
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


           ItemDescriptionNameText.text = itemName;
           ItemDescriptionText.text = itemDescription;
           itemDescriptionImage.sprite = itemSprite != null ? itemSprite : emptySprite;
       }
   }
}


